using Njord.Ais.Enums;
using Njord.Ais.Interfaces;
using Njord.Ais.Messages;
using Njord.Ais.SerDe.JSON;
using Njord.AisStream.Converters;
using System.Text.Json.Serialization;

namespace Njord.AisStream.Messages
{
    public sealed record DataLinkManagementMessage : IDataLinkManagementMessage
    {
        [JsonPropertyName("Data"), JsonConverter(typeof(JsonSlotOffsetInfoArrayConverter))]
        public required IEnumerable<ISlotOffsetInfo> SlotOffsets { get; init; }

        [JsonPropertyName("UserID"), JsonConverter(typeof(JsonIntToMMSIStringConverter))]
        public required string UserId { get; init; }

        [JsonPropertyName("MessageID"), JsonConverter(typeof(JsonCheckedNumberEnumConverter<AisMessageType>))]
        public required AisMessageType MessageId { get; init; }

        [JsonPropertyName("RepeatIndicator"), JsonConverter(typeof(JsonCheckedNumberEnumConverter<RepeatIndicator>))]
        public required RepeatIndicator RepeatIndicator { get; init; }
    }
}
