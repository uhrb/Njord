using Njord.Ais.Interfaces;

namespace Njord.Ais.Messages
{
    public interface ISafetyRelatedBroadcastMessage: IUserId, IMessageId, IRepeatIndicator
    {
        /// <summary>
        /// Safety related text
        /// </summary>
        public string SafetyRelatedText { get; init; }
    }
}
