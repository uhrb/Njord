using Njord.Ais.Interfaces;
using Njord.Ais.SerDe.JSON;
using System.Text.Json.Serialization;

namespace Njord.AisStream.ModelTypes
{
    public record AcknowledgementDestination : IAcknowledgementDestination
    {
        [JsonPropertyName("DestinationID"), JsonConverter(typeof(JsonIntToMMSIStringConverter))]
        public required string DestinationId { get; init; }

        [JsonPropertyName("Sequenceinteger")]
        public required byte SequenceNumber { get; init; }
    }
}
