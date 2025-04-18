using Njord.Ais.Interfaces;

namespace Njord.Ais.Messages
{
    public interface IAddressedSafetyMessage : ISafetyRelatedBroadcastMessage, IDestinationId, IRetransmitFlag, ISequenceNumber
    {
    }
}
