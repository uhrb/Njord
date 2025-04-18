using Njord.Ais.Interfaces;

namespace Njord.Ais.Messages
{
    /// <summary>
    /// Addressed binary message. For Unification, SourceID from original message is going to UserId of the message.
    /// </summary>
    public interface IAddressedBinaryMessage : IUserId, IMessageId, IRepeatIndicator, ISequenceNumber, IDestinationId, IRetransmitFlag
    {
        /// <summary>
        /// Application identifier <see cref="IApplicationIdentifier"/> for details
        /// </summary>
        public IApplicationIdentifier ApplicationIdentifier { get; init; }

        /// <summary>
        /// Application specific data
        /// </summary>
        public ReadOnlyMemory<byte> ApplicationData { get; init; }
    }
}
