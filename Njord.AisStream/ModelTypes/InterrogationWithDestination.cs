using Njord.Ais.Enums;
using Njord.Ais.Interfaces;

namespace Njord.AisStream.ModelTypes
{
    public record InterrogationWithDestination : IInterrogationWithDestination
    {
        public required ushort SlotOffset { get; init; }

        public required AisMessageType MessageType { get; init; }

        public required string DestinationId { get; init; }
    }
}
