using Njord.Ais.Enums;

namespace Njord.Ais.Interfaces
{
    /// <summary>
    /// Incremental time division multiple access communication state
    /// </summary>
    public interface ICommunicationStateITDMA
    {
        /// <summary>
        /// Sync state. See <see cref="CommunicationSyncState"/> for details
        /// </summary>
        public CommunicationSyncState SyncState { get; init; }

        /// <summary>
        /// Offset to next slot to be used, or zero (0) if no more transmissions 
        /// </summary>
        public ushort SlotIncrement { get;init; }

        /// <summary>
        /// <para>Number of consecutive slots to allocate.  </para>
        /// <code>
        /// ╒═══════════════╤══════════════════════════════╕
        /// │ Value         │ Description                  │
        /// ╞═══════════════╪══════════════════════════════╡
        /// │     0         │ 1 slot                       │ 
        /// ├───────────────┼──────────────────────────────┤
        /// │     1         │ 2 slots                      │ 
        /// ├───────────────┼──────────────────────────────┤
        /// │     2         │ 3 slots                      │
        /// ├───────────────┼──────────────────────────────┤
        /// │     3         │ 4 slots                      │
        /// ├───────────────┼──────────────────────────────┤
        /// │     4         │ 5 slots                      │
        /// ├───────────────┼──────────────────────────────┤
        /// │     5         │ 1 slot; offset=slotInc+8192  │
        /// ├───────────────┼──────────────────────────────┤
        /// │     6         │ 2 slot; offset=slotInc+8192  │
        /// ├───────────────┼──────────────────────────────┤
        /// │     7         │ 3 slot; offset=slotInc+8192  │
        /// └───────────────┴──────────────────────────────┘
        /// </code>
        /// Use of 5 to 7 removes the need for RATDMA broadcast for scheduled
        /// transmissions up to 6 min intervals
        /// </para>
        /// </summary>
        public byte NumberOfSlots { get; init; }

        /// <summary>
        /// Set to TRUE = 1 if the slot remains allocated for one additional frame
        /// </summary>
        public bool KeepFlag { get; init; }
    }
}
