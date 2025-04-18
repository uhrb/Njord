using Njord.Ais.Enums;

namespace Njord.Ais.Interfaces
{
    /// <summary>
    /// Destination description for addressed messages
    /// </summary>
    public interface IInterrogationWithDestination : IDestinationId
    {
        /// <summary>
        /// Response slot offset for requested message from interrogated station
        /// </summary>
        public ushort SlotOffset{ get; init; }

        /// <summary>
        /// Requested message type. See <see cref="AisMessageType"/> for message Ids."/>
        /// </summary>
        public AisMessageType MessageType { get; init; }
    }
}
