namespace Njord.Ais.Enums
{
    /// <summary>
    /// Special manoeuvre engagement indicator.
    /// <code>
    /// ╒═══════════════════════════════════════════╤═══════════╕
    /// │ Name                                      │ Value     │
    /// ╞═══════════════════════════════════════════╪═══════════╡
    /// │ NotAvailiable                             │     0     │
    /// ├───────────────────────────────────────────┼───────────┤
    /// │ NotEngaged                                │     1     │
    /// ├───────────────────────────────────────────┼───────────┤
    /// │ Engaged                                   │     2     │
    /// └───────────────────────────────────────────┴───────────┘
    /// </code>
    /// </summary>
    public enum SpecialManoeuvreIndicator : byte
    {
        /// <summary>
        /// Not availiable
        /// </summary>
        NotAvailable = 0,

        /// <summary>
        /// Not engaged
        /// </summary>
        NotEngaged = 1,

        /// <summary>
        /// Engaged
        /// </summary>
        Engaged = 2
    }
}