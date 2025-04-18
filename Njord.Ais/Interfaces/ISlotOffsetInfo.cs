namespace Njord.Ais.Interfaces
{

    /// <summary>
    /// Slot offset information for Data Link Management message
    /// </summary>
    public interface ISlotOffsetInfo
    {
        /// <summary>
        /// Reserved offset number; 0 = not available
        /// </summary>
        public ushort SlotOffset { get; init; }

        /// <summary>
        /// Number of reserved consecutive slots: 1-15;
        /// 0 = not available(1)
        /// </summary>
        public byte NumberOfSlots { get; init; }

        /// <summary>
        /// Time-out value in minutes; 0 = not available(1
        /// </summary>
        public byte TimeoutMinutes { get; init; }

        /// <summary>
        /// Increment to repeat reservation block 1;
        /// 0 = one reservation block per frame(1)
        /// </summary>
        public ushort Increment { get; init; }
    }
}
