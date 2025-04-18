namespace Njord.AisStream
{
    public record AisStreamRawMessageSourceOptions
    {
        public required string ApiKey { get; init; }
        public required Uri Uri { get; init; }
        public required double[][][] BoundingBoxes { get; init; }
        public required string[] FilterMessageTypes { get; init; }
    }
}
