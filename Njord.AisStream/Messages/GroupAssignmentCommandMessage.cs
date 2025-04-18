using Njord.Ais.Enums;
using Njord.Ais.Interfaces;
using Njord.Ais.Messages;
using Njord.AisStream.MessageConverters;
using System.Text.Json.Serialization;

namespace Njord.AisStream.Messages
{
    [JsonConverter(typeof(JsonGroupAssignmentCommandMessageConverter))]
    public sealed record GroupAssignmentCommandMessage : IGroupAssignmentCommandMessage
    {
        public required StationType StationType { get; init; }
        public required ReportingInterval ReportingInterval { get; init; }
        public required byte QuietTime { get; init; }
        public required string UserId { get; init; }
        public required AisMessageType MessageId { get; init; }
        public required RepeatIndicator RepeatIndicator { get; init; }
        public required TypeOfShipAndCargoType TypeOfShipAndCargoType { get; init; }
        public required TxRxModeType TxRxMode { get; init; }
        public required IGeoArea GeoArea { get; init; }
    }
}
