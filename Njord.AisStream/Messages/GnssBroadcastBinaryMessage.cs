using Njord.Ais.Enums;
using Njord.Ais.Messages;
using Njord.Ais.SerDe.JSON;
using System.Text.Json.Serialization;

namespace Njord.AisStream.Messages
{
    public sealed record GnssBroadcastBinaryMessage : IGnssBroadcastBinaryMessage
    {
        [JsonPropertyName("Data"), JsonConverter(typeof(JsonStringToBinaryDataConverter))]
        public required ReadOnlyMemory<byte> DifferentialCorrectionData { get; init; }

        [JsonPropertyName("UserID"), JsonConverter(typeof(JsonIntToMMSIStringConverter))]
        public required string UserId { get; init; }

        [JsonPropertyName("MessageID"), JsonConverter(typeof(JsonCheckedNumberEnumConverter<AisMessageType>))]
        public required AisMessageType MessageId { get; init; }

        [JsonPropertyName("RepeatIndicator"), JsonConverter(typeof(JsonCheckedNumberEnumConverter<RepeatIndicator>))]
        public required RepeatIndicator RepeatIndicator { get; init; }

        [JsonPropertyName("Longitude")]
        public required double Longitude { get; init; }

        [JsonPropertyName("Latitude")]
        public required double Latitude { get; init; }
    }
}
