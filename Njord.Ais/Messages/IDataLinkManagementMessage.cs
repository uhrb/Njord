using Njord.Ais.Interfaces;

namespace Njord.Ais.Messages
{
    /// <summary>
    /// <para>
    /// This message should be used by base station(s) to pre-announce the fixed allocation schedule
    /// (FATDMA) for one or more base station(s) and it should be repeated as often as required. This way
    /// the system can provide a high level of integrity for base station(s). This is especially important in
    /// regions where several base stations are located adjacent to each other and mobile station(s) move
    /// between these different regions.These reserved slots cannot be autonomously allocated by mobile
    /// stations.
    /// </para>
    /// <para>
    /// Source Id is stored in UserId for consistency
    /// </para>
    /// </summary>
    public interface IDataLinkManagementMessage : IUserId, IMessageId, IRepeatIndicator
    {
        public IEnumerable<ISlotOffsetInfo> SlotOffsets { get; init; }
    }
}
