using Njord.Ais.Interfaces;
using Njord.Ais.SerDe.JSON;
using System.Text.Json.Serialization;

namespace Njord.AisStream.ModelTypes
{
    public record AssignedDestination : IAssignedDestination
    {
        [JsonPropertyName("Offset")]
        public required ushort Offset { get; init; }

        [JsonPropertyName("Increment")]
        public required ushort Increment { get; init; }

        [JsonPropertyName("DestinationID"), JsonConverter(typeof(JsonIntToMMSIStringConverter))]
        public required string DestinationId { get; init; }
    }
}
