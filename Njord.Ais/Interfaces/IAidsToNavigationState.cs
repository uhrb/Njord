using Njord.Ais.Enums;

namespace Njord.Ais.Interfaces
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
    public interface IAidsToNavigationState
    {
        /// <summary>
        /// True if in Alarm state, otherwise healthy and false  
        /// </summary>
        public bool IsAlarmState{ get; init; }

        /// <summary>
        /// Describes the light status of the AtoN. See <see cref="AidsToNavigationLightsStatus"/>
        /// </summary>
        public AidsToNavigationLightsStatus LightsState { get; init; }

        /// <summary>
        /// Describes the RACON status of the AtoN. See <see cref="AidsToNavigationRACONStatus"/>
        /// </summary>
        public AidsToNavigationRACONStatus RACONState { get; init; }
    }
}
