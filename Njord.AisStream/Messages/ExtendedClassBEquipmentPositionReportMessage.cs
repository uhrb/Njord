using Njord.Ais.Enums;
using Njord.Ais.Interfaces;
using Njord.Ais.Messages;
using Njord.Ais.SerDe.JSON;
using Njord.AisStream.ModelTypes;
using System.Text.Json.Serialization;

namespace Njord.AisStream.Messages
{
    public sealed record ExtendedClassBEquipmentPositionReportMessage : IExtendedClassBEquipmentPositionReportMessage
    {
        [JsonPropertyName("MessageID"), JsonConverter(typeof(JsonCheckedNumberEnumConverter<AisMessageType>))]
        public required AisMessageType MessageId { get; init; }

        [JsonPropertyName("RepeatIndicator"), JsonConverter(typeof(JsonCheckedNumberEnumConverter<RepeatIndicator>))]
        public required RepeatIndicator RepeatIndicator { get; init; }

        [JsonPropertyName("UserID"), JsonConverter(typeof(JsonIntToMMSIStringConverter))]
        public required string UserId { get; init; }

        [JsonPropertyName("Sog")]
        public required double SpeedOverGround { get; init; }

        [JsonPropertyName("PositionAccuracy")]
        public required bool IsPositionAccuracyHigh { get; init; }

        [JsonPropertyName("Longitude")]
        public required double Longitude { get; init; }

        [JsonPropertyName("Latitude")]
        public required double Latitude { get; init; }

        [JsonPropertyName("Cog")]
        public required double CourseOverGround { get; init; }

        [JsonPropertyName("TrueHeading")]
        public required int TrueHeading { get; init; }

        [JsonPropertyName("Timestamp")]
        public required uint Timestamp { get; init; }

        [JsonPropertyName("Name")]
        public required string Name { get; init; }

        [JsonPropertyName("Type"), JsonConverter(typeof(JsonCheckedNumberEnumConverter<TypeOfShipAndCargoType>))]
        public required TypeOfShipAndCargoType TypeOfShipAndCargoType { get; init; }

        [JsonPropertyName("Dimension"), JsonConverter(typeof(JsonInterfaceFactoryConverter<IDimensions, Dimensions>))]
        public required IDimensions Dimensions { get; init; }

        [JsonPropertyName("FixType"), JsonConverter(typeof(JsonCheckedNumberEnumConverter<PositionFixingDeviceType>))]
        public required PositionFixingDeviceType FixingDeviceType { get; init; }

        [JsonPropertyName("Raim")]
        public required bool IsRaimInUse { get; init; }

        [JsonPropertyName("Dte")]
        public required bool IsDataTerminalEquipmentAvailiable { get; init; }

        [JsonPropertyName("AssignedMode")]
        public required bool IsInAssignedMode { get; init; }
    }
}
