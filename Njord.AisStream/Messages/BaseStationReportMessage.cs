using Njord.Ais.Enums;
using Njord.Ais.Messages;
using Njord.Ais.SerDe.JSON;
using System.Text.Json.Serialization;

namespace Njord.AisStream.Messages
{
    public sealed record BaseStationReportMessage : IBaseStationReportMessage
    {
        [JsonPropertyName("UtcYear")]
        public required ushort UTCYear { get; init; }

        [JsonPropertyName("UtcMonth")]
        public required byte UTCMonth { get; init; }

        [JsonPropertyName("UtcDay")]
        public required byte UTCDay { get; init; }

        [JsonPropertyName("UtcHour")]
        public required byte UTCHour { get; init; }

        [JsonPropertyName("UtcMinute")]
        public required byte UTCMinute { get; init; }

        [JsonPropertyName("UtcSecond")]
        public required byte UTCSecond { get; init; }

        [JsonPropertyName("MessageID"), JsonConverter(typeof(JsonCheckedNumberEnumConverter<AisMessageType>))]
        public required AisMessageType MessageId { get; init; }

        [JsonPropertyName("Longitude")]
        public required double Longitude { get; init; }

        [JsonPropertyName("Latitude")]
        public required double Latitude { get; init; }

        [JsonPropertyName("CommunicationState")]
        public required uint CommunicationState { get; init; }

        [JsonPropertyName("FixType"), JsonConverter(typeof(JsonCheckedNumberEnumConverter<PositionFixingDeviceType>))]
        public required PositionFixingDeviceType FixingDeviceType { get; init; }

        [JsonPropertyName("PositionAccuracy")]
        public required bool IsPositionAccuracyHigh { get; init; }

        [JsonPropertyName("Raim")]
        public required bool IsRaimInUse { get; init; }

        [JsonPropertyName("UserID"), JsonConverter(typeof(JsonIntToMMSIStringConverter))]
        public required string UserId { get; init; }

        [JsonPropertyName("LongRangeEnable")]
        public required bool AskLongRangeRetransmission { get; init; }
    }
}
