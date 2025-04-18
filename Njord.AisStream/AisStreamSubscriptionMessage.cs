using System.Text.Json.Serialization;

namespace Njord.AisStream
{
    internal record AisStreamSubscriptionMessage
    {
        [JsonPropertyName("Apikey")]
        public required string Apikey { get; init; }
        [JsonPropertyName("BoundingBoxes")]
        public double[][][]? BoundingBoxes { get; init; }
        [JsonPropertyName("FilterMessageTypes")]
        public string[]? FilterMessageTypes { get; init; }
    }
}
