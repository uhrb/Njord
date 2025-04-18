namespace Njord.OpenTelemetry
{
    public sealed record MetricPointSnapshot
    {
        public required string Tags { get; init; }

        public required object Value { get; init; }
    }
}
