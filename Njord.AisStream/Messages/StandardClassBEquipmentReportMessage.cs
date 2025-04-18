using Njord.Ais.Enums;
using Njord.Ais.Messages;
using Njord.Ais.SerDe.JSON;
using System.Text.Json.Serialization;

namespace Njord.AisStream.Messages
{
    public sealed record StandardClassBEquipmentReportMessage : IStandardClassBEquipmentReportMessage
    {
        [JsonPropertyName("UserID"), JsonConverter(typeof(JsonIntToMMSIStringConverter))]
        public required string UserId { get; init; }

        [JsonPropertyName("MessageID"), JsonConverter(typeof(JsonCheckedNumberEnumConverter<AisMessageType>))]
        public required AisMessageType MessageId { get; init; }

        [JsonPropertyName("RepeatIndicator"), JsonConverter(typeof(JsonCheckedNumberEnumConverter<RepeatIndicator>))]
        public required RepeatIndicator RepeatIndicator { get; init; }

        [JsonPropertyName("Timestamp")]
        public required uint Timestamp { get; init; }

        [JsonPropertyName("CommunicationState")]
        public required uint CommunicationState { get; init; }

        [JsonPropertyName("Sog")]
        public required double SpeedOverGround { get; init; }

        [JsonPropertyName("Cog")]
        public required double CourseOverGround { get; init; }

        [JsonPropertyName("Longitude")]
        public required double Longitude { get; init; }

        [JsonPropertyName("Latitude")]
        public required double Latitude { get; init; }

        [JsonPropertyName("ClassBUnit")]
        public required bool IsCSUnit { get; init; }

        [JsonPropertyName("ClassBDisplay")]
        public required bool IsDisplayAvailiable { get; init; }

        [JsonPropertyName("ClassBDsc")]
        public required bool IsDSCEquipmentAvailiable { get; init; }

        [JsonPropertyName("ClassBBand")]
        public required bool IsCapableToOperateOverWholeMarineBand { get; init; }

        [JsonPropertyName("ClassBMsg22")]
        public required bool IsSupportChannelManagement { get; init; }

        [JsonPropertyName("AssignedMode")]
        public required bool IsInAssignedMode { get; init; }

        [JsonPropertyName("PositionAccuracy")]
        public required bool IsPositionAccuracyHigh { get; init; }

        [JsonPropertyName("Raim")]
        public required bool IsRaimInUse { get; init; }

        [JsonPropertyName("CommunicationStateIsItdma")]
        public required bool IsCommunicationStateITDMA { get; init; }
    }
}
