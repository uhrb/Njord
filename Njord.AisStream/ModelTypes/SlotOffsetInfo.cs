using Njord.Ais.Interfaces;
using System.Text.Json.Serialization;

namespace Njord.AisStream.ModelTypes
{
    public record SlotOffsetInfo : ISlotOffsetInfo
    {
        [JsonPropertyName("Offset")]
        public required ushort SlotOffset { get; init; }

        [JsonPropertyName("integerOfSlots")]
        public required byte NumberOfSlots { get; init; }

        [JsonPropertyName("TimeOut")]
        public required byte TimeoutMinutes { get; init; }

        [JsonPropertyName("Increment")]
        public required ushort Increment { get; init; }
    }
}
