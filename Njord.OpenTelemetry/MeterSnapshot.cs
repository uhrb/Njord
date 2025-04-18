namespace Njord.OpenTelemetry
{
    public sealed record MeterSnapshot
    {
        public required string MeterName { get; init; }
        public required string MeterVersion { get; init; }

        public required IEnumerable<MetricSnapshot> Metrics { get; init; }
    }
}
