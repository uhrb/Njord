using Njord.Ais.Enums;
using Njord.Ais.Interfaces;

namespace Njord.Ais.Extensions.Types
{
    public static class AidsToNavigationStateExtensions
    {

        /// <summary>
        /// Describes the status of the Aids to Navigation
        /// <code>
        /// ╒══════╤═══════════════════╤═════════════════════════════════════════╕
        /// │ Bits │ Description       │ Details                                 │
        /// ╞══════╪═══════════════════╪═════════════════════════════════════════╡
        /// │ 2    │ RACON status      │ 00 = no RACON installed                 │
        /// │      │                   │ 01 = RACON installed but not monitored  │
        /// │      │                   │ 10 = RACON operational                  │
        /// │      │                   │ 11 = RACON error                        │
        /// ├──────┼───────────────────┼─────────────────────────────────────────┤
        /// │ 2    │ Light status      │ 00 = no light or no monitoring          │
        /// │      │                   │ 01 = light ON                           │
        /// │      │                   │ 10 = light OFF                          │
        /// │      │                   │ 11 = light error                        │
        /// ├──────┼───────────────────┼─────────────────────────────────────────┤
        /// │ 1    │ Health            │ 0 = good health                         │
        /// │      │                   │ 1 = alarm                               │
        /// └──────┴───────────────────┴─────────────────────────────────────────┘
        /// </code>
        /// </summary>
        public static (AidsToNavigationRACONStatus, AidsToNavigationLightsStatus, bool) DecodeAidsToNavigationStatusFromByte(this byte state)
        {
            var raconBinary = state & 0b_0000_0011;
            var lightStatus = (state & 0b_0000_1100) >> 2;
            var health = (state & 0b_0001_0000) >> 4;
            return ((AidsToNavigationRACONStatus)raconBinary, (AidsToNavigationLightsStatus)lightStatus, health == 1);
        }

        public static byte EncodeAidsToNavigationStatusToByte(this IAidsToNavigationState state)
        {
            var bytes = (byte)state.RACONState;
            bytes = (byte)(bytes | ((byte)state.LightsState << 2));
            bytes = (byte)(bytes | (state.IsAlarmState ?  1 : 0) << 4);
            return bytes;
        }
    }
}
