using Njord.Ais.Enums;
using Njord.Ais.Interfaces;
using Njord.Ais.Messages;
using Njord.AisStream.MessageConverters;
using System.Text.Json.Serialization;

namespace Njord.AisStream.Messages
{
    [JsonConverter(typeof(JsonStaticDataReportMessageConverter))]
    public sealed record StaticDataReportMessage : IStaticDataReportMessage
    {
        public required AisMessageType MessageId { get; init; }
        public required string UserId { get; init; }
        #pragma warning disable CS8766 // Nullability of reference types in return type doesn't match implicitly implemented member (possibly because of nullability attributes).
        public required string? Name { get; init; }
        public required bool IsPartA { get; init; }
        public required string? ManufacturerId { get; init; }
        public required byte? UnitModelCode { get; init; }
        public required uint? UnitSerialNumber { get; init; }
        public required RepeatIndicator RepeatIndicator { get; init; }
        public required TypeOfShipAndCargoType TypeOfShipAndCargoType { get; init; }
        public required IDimensions? Dimensions { get; init; }
        public required PositionFixingDeviceType FixingDeviceType { get; init; }
        public required string? CallSign { get; init; }
        #pragma warning restore CS8766 // Nullability of reference types in return type doesn't match implicitly implemented member (possibly because of nullability attributes).
    }
}
