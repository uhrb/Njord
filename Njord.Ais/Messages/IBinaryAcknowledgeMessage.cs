using Njord.Ais.Interfaces;

namespace Njord.Ais.Messages
{
    /// <summary>
    /// Message 7 should be used as an acknowledgement of up to four Message 6 messages received
    /// (see § 5.3.1, Annex 2) and should be transmitted on the channel, where the addressed message to be
    /// acknowledged was received.
    /// Message 13 should be used as an acknowledgement of up to four Message 12 messages received (see
    /// § 5.3.1, Annex 2) and should be transmitted on the channel, where the addressed message to be
    /// acknowledged was received.
    /// </summary>
    public interface IBinaryAcknowledgeMessage : IUserId, IMessageId, IRepeatIndicator
    {
        /// <summary>
        /// List of acknowledgements
        /// </summary>
        public IEnumerable<IAcknowledgementDestination> Acknowledgements { get; init; }
    }
}
