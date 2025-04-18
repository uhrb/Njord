using Njord.Ais.Interfaces;

namespace Njord.Ais.Messages
{
    /// <summary>
    /// Multi slot binary message
    /// </summary>
    public interface IMultislotBinaryMessage : ISingleSlotBinaryMessage, ICommunicationState, ICommunicationStateSelector
    {
    }
}
