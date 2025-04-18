using Njord.Ais.Enums;
using Njord.Ais.Interfaces;
using Njord.Ais.Messages;
using Njord.Ais.SerDe.JSON;
using Njord.AisStream.ModelTypes;
using System.Text.Json.Serialization;

namespace Njord.AisStream.Messages
{
    public sealed record AidsToNavigationReportMessage : IAidsToNavigationReportMessage
    {
        [JsonPropertyName("MessageID"), JsonConverter(typeof(JsonCheckedNumberEnumConverter<AisMessageType>))]
        public required AisMessageType MessageId { get; init; }

        [JsonPropertyName("RepeatIndicator"), JsonConverter(typeof(JsonCheckedNumberEnumConverter<RepeatIndicator>))]
        public required RepeatIndicator RepeatIndicator { get; init; }

        [JsonPropertyName("UserID"), JsonConverter(typeof(JsonIntToMMSIStringConverter))]
        public required string UserId { get; init; }

        [JsonPropertyName("Type"), JsonConverter(typeof(JsonCheckedNumberEnumConverter<AidsToNavigationType>))]
        public required AidsToNavigationType TypeOfAidsToNavigation { get; init; }

        [JsonPropertyName("Name"), JsonConverter(typeof(JsonStringWithTrimConverter))]
        public required string Name { get; init; }

        [JsonPropertyName("NameExtension"), JsonConverter(typeof(JsonStringWithTrimConverter))]
        public required string NameExtension { get; init; }

        [JsonPropertyName("Longitude")]
        public required double Longitude { get; init; }

        [JsonPropertyName("Latitude")]
        public required double Latitude { get; init; }

        [JsonPropertyName("Fixtype"), JsonConverter(typeof(JsonCheckedNumberEnumConverter<PositionFixingDeviceType>))]
        public required PositionFixingDeviceType FixingDeviceType { get; init; }

        [JsonPropertyName("OffPosition")]
        public required bool IsOffPosition { get; init; }

        [JsonPropertyName("VirtualAtoN")]
        public required bool IsVirtualDevice { get; init; }

        [JsonPropertyName("PositionAccuracy")]
        public required bool IsPositionAccuracyHigh { get; init; }

        [JsonPropertyName("Raim")]
        public required bool IsRaimInUse { get; init; }

        [JsonPropertyName("AssignedMode")]
        public required bool IsInAssignedMode { get; init; }

        [JsonPropertyName("Timestamp")]
        public required uint Timestamp { get; init; }

        [JsonPropertyName("AtoN"), JsonConverter(typeof(JsonInterfaceFactoryConverter<IAidsToNavigationState, AidsToNavigationState>))]
        public required IAidsToNavigationState AtoNStatus { get; init; }

        [JsonPropertyName("Dimension"), JsonConverter(typeof(JsonInterfaceFactoryConverter<IDimensions, Dimensions>))]
        public required IDimensions Dimensions { get; init; }
    }
}
