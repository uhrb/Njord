using Njord.Ais.Enums;
using Njord.Ais.Interfaces;

namespace Njord.Ais.Messages
{
    /// <summary>
    /// Represents a long-range AIS broadcast message.
    /// </summary>
    public interface ILongRangeAisBroadcastMessage : IUserId, IMessageId, IRepeatIndicator, INavigationalStatus, IMovingPosition
    {
        /// <summary>
        /// The time of the last position report.
        /// </summary>
        public bool IsPositionLatencyHigherThan5Seconds { get; init; }
    }
}
