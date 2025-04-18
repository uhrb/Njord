using Njord.Ais.Interfaces;

namespace Njord.Ais.Messages
{
    /// <summary>
    /// <para>This message should be used for interrogations via the TDMA (not DSC) VHF data link except for
    /// requests for UTC and date.The response should be transmitted on the channel where the interrogation
    /// was received.
    /// Corresponding paramters are defined and valid only when interrogating a message. For example, if one station 
    /// interrogates another station for a messageId1_1 should be defined but the messageId1_2 should be ommited.
    /// </para>
    /// <para>
    /// For simplicity, Interrogations are defined as a list of InterrogationWithDestination.
    /// </para>
    /// </summary>
    public interface IInterrogationMessage : IUserId, IMessageId, IRepeatIndicator
    {
        /// <summary>
        /// List of requested interrogations
        /// </summary>
        public IEnumerable<IInterrogationWithDestination> Interrogations { get; init; }
    }
}
