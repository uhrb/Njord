using Njord.Ais.Interfaces;
using System.Text.Json.Serialization;

namespace Njord.AisStream.ModelTypes
{
    public sealed record EstimatedTimeOfArrival : IEstimatedTimeOfArrival
    {
        [JsonPropertyName("Month")]
        public required byte Month { get; init; }

        [JsonPropertyName("Day")]
        public required byte Day { get; init; }

        [JsonPropertyName("Hour")]
        public required byte Hour { get; init; }

        [JsonPropertyName("Minute")]
        public required byte Minute { get; init; }
    }
}
