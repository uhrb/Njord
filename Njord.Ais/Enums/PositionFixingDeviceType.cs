namespace Njord.Ais.Enums
{
    /// <summary>
    /// Type of position fixing device
    /// <code>
    /// ╒═══════════════════════════════════════════╤═══════════╕
    /// │ Name                                      │ Value     │
    /// ╞═══════════════════════════════════════════╪═══════════╡
    /// │ Undefined                                 │     0     │
    /// ├───────────────────────────────────────────┼───────────┤
    /// │ GPS                                       │     1     │
    /// ├───────────────────────────────────────────┼───────────┤
    /// │ GLONASS                                   │     2     │
    /// ├───────────────────────────────────────────┼───────────┤
    /// │ CombinedGPSAndGLONASS                     │     3     │
    /// ├───────────────────────────────────────────┼───────────┤
    /// │ LoranC                                    │     4     │
    /// ├───────────────────────────────────────────┼───────────┤
    /// │ Chayka                                    │     5     │
    /// ├───────────────────────────────────────────┼───────────┤
    /// │ IntegratedNavigationSystem                │     6     │
    /// ├───────────────────────────────────────────┼───────────┤
    /// │ Surveyed                                  │     7     │
    /// ├───────────────────────────────────────────┼───────────┤
    /// │ Galileo                                   │     8     │
    /// ├───────────────────────────────────────────┼───────────┤
    /// │ InternalGNSS                              │     9     │ 
    /// └───────────────────────────────────────────┴───────────┘
    /// </code>
    /// </summary>
    public enum PositionFixingDeviceType : byte
    {
        /// <summary>
        /// Undefined
        /// </summary>
        Undefined = 0,

        /// <summary>
        /// GPS
        /// </summary>
        GPS = 1,

        /// <summary>
        /// GLONASS
        /// </summary>
        GLONASS = 2,

        /// <summary>
        /// Combined GPS and GLONASS
        /// </summary>
        CombinedGPSAndGLONASS = 3,

        /// <summary>
        /// Loran-C
        /// </summary>
        LoranC = 4,

        /// <summary>
        /// Chayka
        /// </summary>
        Chayka = 5,

        /// <summary>
        /// Integrated navigation system
        /// </summary>
        IntegratedNavigationSystem = 6,

        /// <summary>
        /// Surveyed
        /// </summary>
        Surveyed = 7,

        /// <summary>
        /// Galileo
        /// </summary>
        Galileo = 8,

        NotUsed9 = 9,
        NotUsed10 = 10, 
        NotUsed11 = 11,
        NotUsed12 = 12,
        NotUsed13 = 13,
        NotUsed14 = 14,

        /// <summary>
        /// Internal GNSS
        /// </summary>
        InternalGNSS = 15
    }
}
