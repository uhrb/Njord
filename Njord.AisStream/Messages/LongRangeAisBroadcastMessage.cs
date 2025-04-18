using Njord.Ais.Enums;
using Njord.Ais.Messages;
using Njord.Ais.SerDe.JSON;
using System.Text.Json.Serialization;

namespace Njord.AisStream.Messages
{
    public sealed record LongRangeAisBroadcastMessage : ILongRangeAisBroadcastMessage
    {
        [JsonPropertyName("PositionLatency")]
        public required bool IsPositionLatencyHigherThan5Seconds { get; init; }
        [JsonPropertyName("UserID"), JsonConverter(typeof(JsonIntToMMSIStringConverter))]
        public required string UserId { get; init; }
        
        [JsonPropertyName("MessageID"), JsonConverter(typeof(JsonCheckedNumberEnumConverter<AisMessageType>))]
        public required AisMessageType MessageId { get; init; }

        [JsonPropertyName("RepeatIndicator"), JsonConverter(typeof(JsonCheckedNumberEnumConverter<RepeatIndicator>))]
        public required RepeatIndicator RepeatIndicator { get; init; }

        [JsonPropertyName("NavigationalStatus"), JsonConverter(typeof(JsonCheckedNumberEnumConverter<NavigationalStatus>))]
        public required NavigationalStatus NavigationalStatus { get; init; }

        [JsonPropertyName("Sog")]
        public required double SpeedOverGround { get; init; }

        [JsonPropertyName("Cog")]
        public required double CourseOverGround { get; init; }

        [JsonPropertyName("Longitude")]
        public required double Longitude { get; init; }

        [JsonPropertyName("Latitude")]
        public required double Latitude { get; init; }

        [JsonPropertyName("PositionAccuracy")]
        public required bool IsPositionAccuracyHigh { get; init; }

        [JsonPropertyName("Raim")]
        public required bool IsRaimInUse { get; init; }
    }
}
