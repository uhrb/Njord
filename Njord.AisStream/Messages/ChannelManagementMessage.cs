using Njord.Ais.Enums;
using Njord.Ais.Interfaces;
using Njord.Ais.Messages;
using Njord.AisStream.MessageConverters;
using System.Text.Json.Serialization;

namespace Njord.AisStream.Messages
{
    [JsonConverter(typeof(JsonChannelManagementMessageConverter))]
    public sealed record ChannelManagementMessage : IChannelManagementMessage
    {
        public required string UserId { get; init; }
        public required AisMessageType MessageId { get; init; }
        public required RepeatIndicator RepeatIndicator { get; init; }
        public required ushort ChannelA { get; init; }
        public required ushort ChannelB { get; init; }
        public required TxRxModeType TxRxMode { get; init; }
        public bool IsLowPower { get; init; }
        public required byte TransitionalZoneSize { get; init; }
        public required bool IsAddressed { get; init; }
        public required bool IsChannelABandwidthSpare { get; init; }
        public required bool IsChannelBBandwidthSpare { get; init; }
        public required IGeoArea? Area { get; init; }
        public required IEnumerable<string>? AddressedStations { get; init; }
    }
}
