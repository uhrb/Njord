using Microsoft.Extensions.Caching.Memory;
using OpenTelemetry;
using OpenTelemetry.Metrics;

namespace Njord.OpenTelemetry
{
    public sealed class MemoryCacheExporter : BaseExporter<Metric>
    {
        private readonly IMemoryCache _cache;
        private readonly string _cacheKey;

        public MemoryCacheExporter(IMemoryCache cache, string cacheKey)
        {
            _cache = cache;
            _cacheKey = cacheKey;
        }

        public override ExportResult Export(in Batch<Metric> batch)
        {
            var lst = new List<Metric>();
            foreach (var metric in batch)
            {
                lst.Add(metric);
            }

            var meters = lst.GroupBy(_ => _.MeterName).SelectMany(TransformMeterNameGroup).ToArray();

            _cache.Set(_cacheKey, meters);
            return ExportResult.Success;
        }

        private static IEnumerable<MeterSnapshot> TransformMeterNameGroup(IGrouping<string, Metric> meterNameGroup)
        {

            var versionGroup = meterNameGroup.GroupBy(_ => _.MeterVersion);
            foreach (var version in versionGroup)
            {
                yield return new MeterSnapshot
                {
                    MeterName = meterNameGroup.Key,
                    MeterVersion = version.Key,
                    Metrics = [.. version.Select(TransformMetric)]
                };
            }
        }

        private static MetricSnapshot TransformMetric(Metric source)
        {
            var points = new List<MetricPoint>();
            foreach (var point in source.GetMetricPoints())
            {
                points.Add(point);
            }

            return new MetricSnapshot
            {
                MetricName = source.Name,
                MetricType = Enum.GetName(source.MetricType)!,
                Points = points.Select(_ => TransformPoint(_, source.MetricType)).ToArray()
            };
        }

        private static MetricPointSnapshot TransformPoint(MetricPoint point, MetricType metricType)
        {
            switch (metricType)
            {
                case MetricType.DoubleGauge:
                    return new MetricPointSnapshot { Tags = ConvertTags(point.Tags), Value = point.GetGaugeLastValueDouble() };
                case MetricType.LongGauge:
                    return new MetricPointSnapshot { Tags = ConvertTags(point.Tags), Value = point.GetGaugeLastValueLong() };
                case MetricType.DoubleSum:
                case MetricType.DoubleSumNonMonotonic:
                    return new MetricPointSnapshot { Tags = ConvertTags(point.Tags), Value = point.GetSumDouble() };
                case MetricType.LongSum:
                case MetricType.LongSumNonMonotonic:
                    return new MetricPointSnapshot { Tags = ConvertTags(point.Tags), Value = point.GetSumLong() };
                case MetricType.Histogram:
                case MetricType.ExponentialHistogram:
                    return new MetricPointSnapshot { Tags = ConvertTags(point.Tags), Value = point.GetHistogramSum() };
                default:
                    throw new NotSupportedException();
            }
        }

        private static string ConvertTags(ReadOnlyTagCollection tags)
        {
            var lst = new List<string>();
            foreach (var tag in tags)
            {
                lst.Add($"{tag.Key}={tag.Value}");
            }
            return string.Join(",", lst);
        }
    }
}
