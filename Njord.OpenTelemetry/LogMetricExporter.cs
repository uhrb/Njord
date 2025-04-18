using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using System.Collections.Concurrent;
using System.Globalization;
using System.Text;

namespace Njord.OpenTelemetry
{
    public sealed class LogMetricExporter : BaseExporter<Metric>
    {
        private readonly ILogger<LogMetricExporter> _logger;
        private readonly ConcurrentDictionary<string, decimal> _previous = [];
        private DateTime _previousTime = DateTime.Now;

        public LogMetricExporter(ILogger<LogMetricExporter> logger)
        {
            _logger = logger;
        }

        private sealed record MeterMetrics
        {
            public required string MeterName { get; init; }
            public required string MeterVersion { get; init; }
            public required Dictionary<string, MetricValue> Metrics { get; init; }
        }

        private sealed record MetricValue
        {
            public required string MetricName { get; init; }
            public required string MetricDescription { get; init; }
            public required string? MetricUnit { get; init; }
            public required List<MetricValueEntry> Entries { get; init; }
        }

        private sealed record MetricValueEntry
        {
            public required ReadOnlyTagCollection Tags { get; init; }
            public required string DisplayValue { get; init; }
        }

        public override ExportResult Export(in Batch<Metric> batch)
        {
            var dicMeters = new Dictionary<string, MeterMetrics>();
            var ellapsedSeconds = (DateTime.Now - _previousTime).TotalSeconds;
            foreach (var metric in batch)
            {
                var meterKey = $"{metric.MeterName} - {metric.MeterVersion}";
                if (false == dicMeters.ContainsKey(meterKey))
                {
                    dicMeters.Add(
                        meterKey,
                        new MeterMetrics
                        {
                            MeterName = metric.MeterName,
                            MeterVersion = metric.MeterVersion,
                            Metrics = []
                        });
                }

                var meterEntry = dicMeters[meterKey];

                if (false == meterEntry.Metrics.ContainsKey(metric.Name))
                {
                    meterEntry.Metrics.Add(
                        metric.Name,
                        new MetricValue
                        {
                            MetricName = metric.Name,
                            MetricUnit = metric.Unit,
                            MetricDescription = metric.Description,
                            Entries = []
                        });
                }

                var metricEntry = meterEntry.Metrics[metric.Name];

                foreach (ref readonly var metricPoint in metric.GetMetricPoints())
                {

                    var metricType = metric.MetricType;
                    if (metricType == MetricType.Histogram || metricType == MetricType.ExponentialHistogram)
                    {
                        var sum = metricPoint.GetHistogramSum();
                        var count = metricPoint.GetHistogramCount();
                        metricEntry.Entries.Add(new MetricValueEntry { Tags = metricPoint.Tags, DisplayValue = $"Sum: {sum} Count: {count} " });
                        if (metricPoint.TryGetHistogramMinMaxValues(out double min, out double max))
                        {
                            metricEntry.Entries.Add(new MetricValueEntry { DisplayValue = $"Min: {min} Max: {max} ", Tags = metricPoint.Tags });
                        }


                        if (metricType == MetricType.Histogram)
                        {
                            bool isFirstIteration = true;
                            double previousExplicitBound = default;
                            foreach (var histogramMeasurement in metricPoint.GetHistogramBuckets())
                            {
                                if (isFirstIteration)
                                {
                                    metricEntry.Entries.Add(new MetricValueEntry
                                    {
                                        DisplayValue = $"(-Infinity,{histogramMeasurement.ExplicitBound}]:{histogramMeasurement.BucketCount}",
                                        Tags = metricPoint.Tags
                                    });
                                    previousExplicitBound = histogramMeasurement.ExplicitBound;
                                    isFirstIteration = false;
                                }
                                else
                                {
                                    metricEntry.Entries.Add(new MetricValueEntry
                                    {
                                        DisplayValue = $"({previousExplicitBound},{(histogramMeasurement.ExplicitBound != double.PositiveInfinity ? histogramMeasurement.ExplicitBound : "+Infinity")}]:{histogramMeasurement.BucketCount}",
                                        Tags = metricPoint.Tags
                                    });
                                    if (histogramMeasurement.ExplicitBound != double.PositiveInfinity)
                                    {
                                        previousExplicitBound = histogramMeasurement.ExplicitBound;
                                    }
                                }

                            }
                        }
                        else
                        {
                            var exponentialHistogramData = metricPoint.GetExponentialHistogramData();
                            var scale = exponentialHistogramData.Scale;

                            if (exponentialHistogramData.ZeroCount != 0)
                            {
                                metricEntry.Entries.Add(new MetricValueEntry { Tags = metricPoint.Tags, DisplayValue = $"Zero Bucket:{exponentialHistogramData.ZeroCount}" });
                            }

                            var offset = exponentialHistogramData.PositiveBuckets.Offset;
                            foreach (var bucketCount in exponentialHistogramData.PositiveBuckets)
                            {
                                var lowerBound = CalculateLowerBoundary(offset, scale).ToString(CultureInfo.InvariantCulture);
                                var upperBound = CalculateLowerBoundary(++offset, scale).ToString(CultureInfo.InvariantCulture);
                                metricEntry.Entries.Add(new MetricValueEntry { Tags = metricPoint.Tags, DisplayValue = $"({lowerBound}, {upperBound}]:{bucketCount}" });
                            }
                        }
                    }
                    else if (metricType.IsDouble())
                    {
                        if (metricType.IsSum())
                        {
                            var key = $"{metric.Name}{ConvertTags(metricPoint.Tags)}";
                            var currentValue = (decimal)metricPoint.GetSumDouble();
                            decimal prev = 0;
                            var diff = currentValue - prev;
                            var perSec = ((double)diff / ellapsedSeconds).ToString("0.##");
                            _previous.AddOrUpdate(key, currentValue, (k, v) =>
                            {
                                prev = v;
                                return currentValue;
                            });
                            metricEntry.Entries.Add(new MetricValueEntry { DisplayValue = $"{currentValue,-8} {(diff < 0 ? '-' : '+')}{diff,-8}{perSec,8}/sec", Tags = metricPoint.Tags });
                        }
                        else
                        {
                            metricEntry.Entries.Add(new MetricValueEntry { DisplayValue = metricPoint.GetGaugeLastValueDouble().ToString(), Tags = metricPoint.Tags });
                        }
                    }
                    else if (metricType.IsLong())
                    {
                        if (metricType.IsSum())
                        {
                            var key = $"{metric.Name}{ConvertTags(metricPoint.Tags)}";
                            var currentValue = (decimal)metricPoint.GetSumLong();
                            decimal prev = 0;
                            _previous.AddOrUpdate(key, currentValue, (k, v) =>
                            {
                                prev = v;
                                return currentValue;
                            });
                            var diff = currentValue - prev;
                            var perSec = ((double)diff / ellapsedSeconds).ToString("0.##");
                            metricEntry.Entries.Add(new MetricValueEntry { DisplayValue = $"{currentValue,-8} {(diff < 0 ? '-' : '+')}{diff,-8}{perSec,8}/sec", Tags = metricPoint.Tags });
                        }
                        else
                        {
                            metricEntry.Entries.Add(new MetricValueEntry { DisplayValue = metricPoint.GetGaugeLastValueLong().ToString(), Tags = metricPoint.Tags });
                        }
                    }
                }
            }

            var output = new StringBuilder();

            foreach (var kvp in dicMeters)
            {
                output.AppendLine($"{kvp.Key}");
                foreach (var metric in kvp.Value.Metrics)
                {
                    output.AppendLine($"└─{metric.Value.MetricName}");
                    var tagCounts = metric.Value.Entries
                       .SelectMany(e => EnumerateTags(e.Tags).Select(tag => tag))
                       .GroupBy(tag => tag)
                       .ToDictionary(g => g.Key, g => g.Count());

                    // Sort tag keys by frequency in descending order
                    var sortedTagKeys = tagCounts
                        .OrderByDescending(kvp => kvp.Value)
                        .Select(kvp => kvp.Key.Key)
                        .Distinct()
                        .ToList();

                    // Build the tree
                    var root = new TagTreeNode { Name = "Root" };
                    foreach (var entry in metric.Value.Entries)
                    {
                        AddToTree(root, entry, sortedTagKeys);
                    }

                    PrintTree(root, " ", output);
                }
            }

            _logger.LogInformation(output.ToString());
            _previousTime = DateTime.Now;
            return ExportResult.Success;
        }

        static void PrintTree(TagTreeNode node, string indent, StringBuilder sb)
        {
            if (node.Name != "Root")
            {
                var s = indent + "└" + node.Name;
                if(node.Children.Count == 0)
                {
                    foreach (var displayValue in node.DisplayValues)
                    {
                        s = s.PadRight(50) + displayValue;
                    }
                } 

                sb.AppendLine(s);
            }

            foreach (var child in node.Children.Values)
            {
                PrintTree(child, indent + " ", sb);
            }
        }

        private sealed record TagTreeNode
        {
            public required string Name { get; set; }
            public Dictionary<string, TagTreeNode> Children { get; init; } = [];
            public List<string> DisplayValues { get; init; } = [];
        }

        private static void AddToTree(TagTreeNode node, MetricValueEntry entry, List<string> sortedTagKeys)
        {
            var current = node;

            // Sort the entity's tags based on the global sorted order
            var sortedTags = EnumerateTags(entry.Tags)
                .OrderBy(kv => sortedTagKeys.IndexOf(kv.Key))  // Preserve the global frequency sorting
                .ToList();

            foreach (var (key, value) in sortedTags)
            {
                var tagString = $"{key}={value}";
                if (!current.Children.ContainsKey(tagString))
                {
                    current.Children[tagString] = new TagTreeNode { Name = tagString };
                }
                current = current.Children[tagString];
            }

            // Store the display value at the deepest level
            current.DisplayValues.Add(entry.DisplayValue);
        }

        private static IEnumerable<KeyValuePair<string, object?>> EnumerateTags(ReadOnlyTagCollection tags)
        {
            foreach (var tag in tags)
            {
                yield return new KeyValuePair<string, object?>(tag.Key, tag.Value);
            }
        }

        private static string ConvertTags(ReadOnlyTagCollection tags)
        {
            var lst = new List<string>();
            foreach (var tag in tags)
            {
                lst.Add($"{tag.Key}={tag.Value}".PadRight(40));
            }

            return string.Join("; ", lst).PadRight(50);
        }

        private const double _epsilonTimes2 = double.Epsilon * 2;
        private static readonly double _ln2 = Math.Log(2);

        private static double CalculateLowerBoundary(int index, int scale)
        {
            if (scale > 0)
            {
                var inverseFactor = Math.ScaleB(_ln2, -scale);
                var lowerBound = Math.Exp(index * inverseFactor);
                return lowerBound == 0 ? double.Epsilon : lowerBound;
            }
            else
            {
                if (scale == -1 && index == -537 || scale == 0 && index == -1074)
                {
                    return _epsilonTimes2;
                }

                var n = index << -scale;

                // LowerBoundary should not return zero.
                // It should return values >= double.Epsilon (2 ^ -1074).
                // n < -1074 occurs at the minimum index of a scale.
                // e.g., At scale -1, minimum index is -538. -538 << 1 = -1075
                if (n < -1074)
                {
                    return double.Epsilon;
                }

                return Math.ScaleB(1, n);
            }
        }
    }
}
