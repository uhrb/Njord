using Njord.Ais.Enums;
using Njord.Ais.Interfaces;
using Njord.Ais.Messages;
using Njord.Ais.SerDe.JSON;
using Njord.AisStream.ModelTypes;
using System.Text.Json.Serialization;

namespace Njord.AisStream.Messages
{
    public sealed record ShipStaticAndVoyageRelatedDataMessage : IShipStaticAndVoyageRelatedDataMessage
    {
        [JsonPropertyName("MessageID"), JsonConverter(typeof(JsonCheckedNumberEnumConverter<AisMessageType>))]
        public required AisMessageType MessageId { get; init; }

        [JsonPropertyName("RepeatIndicator"), JsonConverter(typeof(JsonCheckedNumberEnumConverter<RepeatIndicator>))]
        public required RepeatIndicator RepeatIndicator { get; init; }

        [JsonPropertyName("UserID"), JsonConverter(typeof(JsonIntToMMSIStringConverter))]
        public required string UserId { get; init; }

        [JsonPropertyName("AisVersion"), JsonConverter(typeof(JsonCheckedNumberEnumConverter<AisVersion>))]
        public required AisVersion AisVersion { get; init; }

        [JsonPropertyName("ImoNumber")]
        public required uint IMONumber { get; init; }

        [JsonPropertyName("CallSign"), JsonConverter(typeof(JsonStringWithTrimConverter))]
        public required string CallSign { get; init; }

        [JsonPropertyName("Name"), JsonConverter(typeof(JsonStringWithTrimConverter))]
        public required string Name { get; init; }

        [JsonPropertyName("Type"), JsonConverter(typeof(JsonCheckedNumberEnumConverter<TypeOfShipAndCargoType>))]
        public required TypeOfShipAndCargoType TypeOfShipAndCargoType { get; init; }

        [JsonPropertyName("Dimension"), JsonConverter(typeof(JsonInterfaceFactoryConverter<IDimensions, Dimensions>))]
        public required IDimensions Dimensions { get; init; }

        [JsonPropertyName("FixType"), JsonConverter(typeof(JsonCheckedNumberEnumConverter<PositionFixingDeviceType>))]
        public required PositionFixingDeviceType FixingDeviceType { get; init; }

        [JsonPropertyName("MaximumStaticDraught")]
        public required float MaximumPresentStaticDraught { get; init; }

        [JsonPropertyName("Destination")]
        public required string Destination { get; init; }

        [JsonPropertyName("Dte")]
        public required bool IsDataTerminalEquipmentAvailiable { get; init; }

        [JsonPropertyName("Eta"), JsonConverter(typeof(JsonInterfaceFactoryConverter<IEstimatedTimeOfArrival, EstimatedTimeOfArrival>))]
        public required IEstimatedTimeOfArrival EstimatedTimeOfArrival { get; init; }
    }
}
