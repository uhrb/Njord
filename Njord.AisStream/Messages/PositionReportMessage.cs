using Njord.Ais.Enums;
using Njord.Ais.Messages;
using Njord.Ais.SerDe.JSON;
using Njord.AisStream.Converters;
using System.Text.Json.Serialization;

namespace Njord.AisStream.Messages
{
    public sealed record PositionReportMessage : IPositionReportMessage
    {
        [JsonPropertyName("RateOfTurn")]
        public required sbyte RateOfTurn { get; init; }

        [JsonPropertyName("SpecialManoeuvreIndicator"), JsonConverter(typeof(JsonSpecialManeouvreEnumConverter))]
        public required SpecialManoeuvreIndicator SpecialManoeuvreIndicator { get; init; }

        [JsonPropertyName("UserID"), JsonConverter(typeof(JsonIntToMMSIStringConverter))]
        public required string UserId { get; init; }

        [JsonPropertyName("MessageID"), JsonConverter(typeof(JsonCheckedNumberEnumConverter<AisMessageType>))]
        public required AisMessageType MessageId { get; init; }

        [JsonPropertyName("Timestamp")]
        public required uint Timestamp { get; init; }

        [JsonPropertyName("CommunicationState")]
        public required uint CommunicationState { get; init; }

        [JsonPropertyName("TrueHeading")]
        public required int TrueHeading { get; init; }

        [JsonPropertyName("NavigationalStatus"), JsonConverter(typeof(JsonCheckedNumberEnumConverter<NavigationalStatus>))]
        public required NavigationalStatus NavigationalStatus { get; init; }

        [JsonPropertyName("Longitude")]
        public required double Longitude { get; init; }

        [JsonPropertyName("Latitude")]
        public required double Latitude { get; init; }

        [JsonPropertyName("Sog")]
        public required double SpeedOverGround { get; init; }

        [JsonPropertyName("Cog")]
        public required double CourseOverGround { get; init; }

        [JsonPropertyName("PositionAccuracy")]
        public required bool IsPositionAccuracyHigh { get; init; }

        [JsonPropertyName("Raim")]
        public required bool IsRaimInUse { get; init; }
    }
}
