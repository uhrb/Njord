namespace Njord.Ais.Enums
{
    /// <summary>
    /// <code>
    /// ╒═══════════════════════════════════════════╤═══════════╕
    /// │ Status                                    │ Value     │
    /// ╞═══════════════════════════════════════════╪═══════════╡
    /// │ under way using engine                    │     0     │
    /// ├───────────────────────────────────────────┼───────────┤
    /// │ at anchor                                 │     1     │
    /// ├───────────────────────────────────────────┼───────────┤
    /// │ not under command                         │     2     │
    /// ├───────────────────────────────────────────┼───────────┤
    /// │ restricted maneuverability                │     3     │
    /// ├───────────────────────────────────────────┼───────────┤
    /// │ constrained by her draught                │     4     │
    /// ├───────────────────────────────────────────┼───────────┤
    /// │ moored                                    │     5     │
    /// ├───────────────────────────────────────────┼───────────┤
    /// │ aground                                   │     6     │
    /// ├───────────────────────────────────────────┼───────────┤
    /// │ engaged in fishing                        │     7     │
    /// ├───────────────────────────────────────────┼───────────┤
    /// │ under way sailing                         │     8     │
    /// ├───────────────────────────────────────────┼───────────┤
    /// │ reserved for future amendment of          │           │
    /// │ navigational status for ships carrying DG,│           │
    /// │ HS, or MP, or IMO hazard or pollutant     │     9     │
    /// │ category C, high speed craft(HSC)         │           │
    /// ├───────────────────────────────────────────┼───────────┤
    /// │ reserved for future amendment of          │           │
    /// │ navigational status for ships carrying    │           │
    /// │ dangerous goods(DG), harmful              │     10    │
    /// │ substances(HS) or marine pollutants(MP),  │           │
    /// │ or IMO hazard or pollutant category A,    │           │
    /// │ wing in ground(WIG)                       │           │
    /// ├───────────────────────────────────────────┼───────────┤
    /// │ power-driven vessel towing astern         │           │
    /// │ (regional use)                            │     11    │
    /// ├───────────────────────────────────────────┼───────────┤
    /// │ power-driven vessel pushing ahead or      │           │
    /// │ towing alongside(regional use)            │     12    │
    /// ├───────────────────────────────────────────┼───────────┤
    /// │ reserved for future use,                  │     13    │
    /// ├───────────────────────────────────────────┼───────────┤
    /// │ AIS-SART(active), MOB-AIS, EPIRB-AIS      │     14    │
    /// ├───────────────────────────────────────────┼───────────┤
    /// │ undefined = default (also used by         │           │
    /// │ AIS-SART, MOB-AIS and EPIRB-AIS under     │     15    │
    /// │ test)                                     │           │
    /// └───────────────────────────────────────────┴───────────┘
    /// </code>
    /// </summary>
    public enum NavigationalStatus : byte
    {
        /// <summary>
        /// Under way using engine
        /// </summary>
        UnderWayUsingEngine = 0,

        /// <summary>
        /// At anchor
        /// </summary>
        AtAnchor = 1,

        /// <summary>
        /// Not under command
        /// </summary>
        NotUnderCommand = 2,

        /// <summary>
        /// Restricted maneuverability
        /// </summary>
        RestrictedManeuverability = 3,

        /// <summary>
        /// Constrained by her draught
        /// </summary>
        ConstrainedByHerDraught = 4,

        /// <summary>
        /// Moored
        /// </summary>
        Moored = 5,

        /// <summary>
        /// Aground
        /// </summary>
        Aground = 6,

        /// <summary>
        /// Engaged in fishing
        /// </summary>
        EngagedInFishing = 7,

        /// <summary>
        /// Under way sailing
        /// </summary>
        UnderWaySailing = 8,

        /// <summary>
        /// Reserved for future amendment of navigational status for ships carrying DG, HS, or MP, or IMO hazard or pollutant category C, high speed craft(HSC)
        /// </summary>
        ReservedDGHSMPCategoryCHighSpeedCraft = 9,

        /// <summary>
        /// Reserved for future amendment of navigational status for ships carrying dangerous goods(DG), harmful substances(HS) or marine pollutants(MP), or IMO hazard or pollutant category A, wing in ground(WIG)
        /// </summary>
        ReservedDGHSMPCategoryAWingInGround = 10,

        /// <summary>
        /// Power-driven vessel towing astern (regional use)
        /// </summary>
        PowerDrivenVesselTowingAstern = 11,

        /// <summary>
        /// Power-driven vessel pushing ahead or towing alongside (regional use)
        /// </summary>
        PowerDrivenVesselPushingAheadOrAlongSide = 12,

        /// <summary>
        /// Reserved for future use
        /// </summary>
        Reserved = 13,

        /// <summary>
        /// AIS-SART(active), MOB-AIS, EPIRB-AIS
        /// </summary>
        AisSARTMOBEPIRB = 14,

        /// <summary>
        /// Undefined = default (also used by AIS-SART, MOB-AIS and EPIRB-AIS under test)
        /// </summary>
        Undefined = 15
    }
}
