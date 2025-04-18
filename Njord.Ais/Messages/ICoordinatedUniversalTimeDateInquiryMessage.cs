using Njord.Ais.Interfaces;

namespace Njord.Ais.Messages
{
    /// <summary>
    /// This message should be used when a station is requesting UTC and date from another station.
    /// UserId should be used for SourceId for consistency.
    /// </summary>
    public interface ICoordinatedUniversalTimeDateInquiryMessage : IUserId, IMessageId, IRepeatIndicator, IDestinationId
    {
    }
}
