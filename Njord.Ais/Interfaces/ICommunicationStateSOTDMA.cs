using Njord.Ais.Enums;

namespace Njord.Ais.Interfaces
{
    /// <summary>
    /// Self-organizing time division multiple access communication state 
    /// The communication state provides the following functions: – it contains 
    /// information used by the slot allocation algorithm in the SOTDMA concept;
    /// it also indicates the synchronization state
    /// </summary>
    public interface ICommunicationStateSOTDMA
    {
        /// <summary>
        /// Sync state. See <see cref="CommunicationSyncState"/> for details
        /// </summary>
        public CommunicationSyncState SyncState { get; init; }

        /// <summary>
        /// Specifies frames remaining until a new slot is selected. 
        /// 0 means that this was the last transmission in this slot.
        /// 1-7 means that 1 to 7 frames respectively are left until slot change.
        /// </summary>
        public byte SlotTimeout { get; init; }

        /// <summary>
        /// <para>
        /// The sub message depends on the current value in slot time-out as 
        /// described in table
        /// </para>
        /// <code>
        /// ╒═══════════════╤══════════════════════════════╤═════════════════════════════════════════════════════════╕
        /// │ Slot timeout  │ SubMessage                   │                                                         │
        /// ╞═══════════════╪══════════════════════════════╪═════════════════════════════════════════════════════════╡
        /// │ 3, 5, 7       │ Received stations            │ Number of other stations (not own station) which the    │
        /// │               │                              │ station currently is receiving(between 0 and 16 383).   │
        /// ├───────────────┼──────────────────────────────┼─────────────────────────────────────────────────────────┤
        /// │ 2, 4, 6       │ Slot number                  │ Slot number used for this transmission                  │
        /// │               │                              │ (between 0 and 2 249).                                  │
        /// ├───────────────┼──────────────────────────────┼─────────────────────────────────────────────────────────┤
        /// │ 1             │ UTC hour and minute          │ If the station has access to UTC, the hour and minute   │
        /// │               │                              │ should be indicated in this sub message. Hour(0-23)     │
        /// │               │                              │ should be coded in bits 13 to 9 of the sub              │
        /// │               │                              │ message(bit 13 is MSB). Minute(0-59) should be coded in │
        /// │               │                              │ bit 8 to 2 (bit 8 is MSB). Bit 1 and bit 0 are not used.│
        /// ├───────────────┼──────────────────────────────┼─────────────────────────────────────────────────────────┤
        /// │ 0             │ Slot offset                  │ If the slot time-out value is 0 (zero) then the slot    │
        /// │               │                              │ offset should indicate the offset to the slot in which  │
        /// │               │                              │ transmission will occur  during the next frame. If the  │
        /// │               │                              │ slot offset is zero, the slot should be de-allocated    │
        /// │               │                              │ after transmission.                                     │
        /// └───────────────┴──────────────────────────────┴─────────────────────────────────────────────────────────┘
        /// </code>
        /// </summary>
        public ushort SubMessage { get; init; }
    }
}
