using Njord.Ais.Enums;
using Njord.Ais.Messages;
using Njord.Ais.SerDe.JSON;
using System.Text.Json.Serialization;

namespace Njord.AisStream.Messages
{
    public sealed record SafetyRelatedBroadcastMessage : ISafetyRelatedBroadcastMessage
    {
        [JsonPropertyName("Text"), JsonConverter(typeof(JsonStringWithTrimConverter))]
        public required string SafetyRelatedText { get; init; }

        [JsonPropertyName("UserID"), JsonConverter(typeof(JsonIntToMMSIStringConverter))]
        public required string UserId { get; init; }

        [JsonPropertyName("MessageID"), JsonConverter(typeof(JsonCheckedNumberEnumConverter<AisMessageType>))]
        public required AisMessageType MessageId { get; init; }

        [JsonPropertyName("RepeatIndicator"), JsonConverter(typeof(JsonCheckedNumberEnumConverter<RepeatIndicator>))]
        public required RepeatIndicator RepeatIndicator { get; init; }
    }
}
