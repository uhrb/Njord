namespace Njord.OpenTelemetry
{
    public sealed record MetricSnapshot
    {
        public required string MetricType { get; init; }
        public required string MetricName { get; init; }
        public required IEnumerable<MetricPointSnapshot> Points { get; init; }
    }
}
