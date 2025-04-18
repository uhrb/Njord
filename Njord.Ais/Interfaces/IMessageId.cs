using Njord.Ais.Enums;

namespace Njord.Ais.Interfaces
{
    /// <summary>
    /// Base class for all AIS messages
    /// </summary>
    public interface IMessageId
    {
        /// <summary>
        /// Message ID see <see cref="AisMessageType"/> for details
        /// </summary>
        public AisMessageType MessageId { get; init; }
    }
}
