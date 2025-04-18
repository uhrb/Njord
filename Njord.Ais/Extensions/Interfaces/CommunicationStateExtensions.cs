using Njord.Ais.Enums;
using Njord.Ais.Interfaces;

namespace Njord.Ais.Extensions.Interfaces
{
    public static class CommunicationStateExtensions
    {
        /// <summary>
        /// <para>Decodes communication state from uint to structured representation.</para>
        /// <para>For mre details see returning type documentation</para>
        /// <code>
        /// ╒═══════════════╤════════════════╕
        /// │ Parameter     │ Number of bits │
        /// ╞═══════════════╪════════════════╡
        /// │ Sync state    │ 2              │
        /// ├───────────────┼────────────────┤
        /// │ Slot timeout  │ 3              │ 
        /// ├───────────────┼────────────────┤
        /// │ Sub message   │ 14             │ 
        /// └───────────────┴────────────────┘
        /// </code>
        /// </summary>
        /// <param name="msg">Message, which CommunicationState will be used</param>
        /// <returns>(comState, slotTimeout, subMessage)</returns>
        public static (CommunicationSyncState, byte, ushort) DecodeCommunicationStateSOTDMA(this ICommunicationState msg)
        {
            var syncStateMask = 0b_0000_0000_0000_0000_0000_0000_0000_0011;
            var slotTimeOutMask = 0b_0000_0000_0000_0000_0000_0000_0001_1100;
            var subMessageMask = 0b_0000_0000_0000_0111_1111_1111_1110_0000;
            return (
                (CommunicationSyncState)(msg.CommunicationState & syncStateMask),
                (byte)((msg.CommunicationState & slotTimeOutMask) >> 2),
                (ushort)((msg.CommunicationState & subMessageMask) >> 5)
            );
        }

        /// <summary>
        /// <para>Decode communication state from uint to structured representation</para>
        /// <para>For more details see returning type documentation</para>
        /// <code>
        /// ╒═══════════════╤════════════════╕
        /// │ Parameter     │ Number of bits │
        /// ╞═══════════════╪════════════════╡
        /// │ Sync state    │ 2              │
        /// ├───────────────┼────────────────┤
        /// │ Slot increment│ 13             │ 
        /// ├───────────────┼────────────────┤
        /// │Number of slots│ 3              │ 
        /// ├───────────────┼────────────────┤
        /// │ Keep flag     │ 1              │ 
        /// └───────────────┴────────────────┘
        /// </code>
        /// </summary>
        /// <param name="msg">Message, which CommunicationState will be used</param>
        /// <returns>Decoded communication state</returns>
        public static (CommunicationSyncState, ushort, byte, bool) DecodeCommunicationStateITDMA(this ICommunicationState msg)
        {
            var syncStateMask = 0b_0000_0000_0000_0000_0000_0000_0000_0011;
            var slotIncrementMask = 0b_0000_0000_0000_0000_0111_1111_1111_1100;
            var numberOfSlotsMask = 0b_0000_0000_0000_0011_1000_0000_0000_0000;
            var keepFlag = 0b_0000_0000_0000_0100_0000_0000_0000_0000;
            return (
                (CommunicationSyncState)(msg.CommunicationState & syncStateMask),
                (ushort)((msg.CommunicationState & slotIncrementMask) >> 2),
                (byte)((msg.CommunicationState & numberOfSlotsMask) >> 15),
                (msg.CommunicationState & keepFlag) > 0
            );
        }
    }
}
