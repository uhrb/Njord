namespace Njord.Ais.Enums
{
    /// <summary>
    /// When mobile station is transmitting a message, it should always set the repeat indicator to default = 0.
    /// The repeat indicator should be increased whenever the transmitted message is a repeat of a message 
    /// already transmitted from another station.
    /// When a base station is used to transmit messages on behalf of another entity(authority, AtoN,
    /// or a virtual or synthetic AtoN), that uses an MMSI other than the base station’s own MMSI, the repeat
    /// indicator of the transmitted message should be set to a non-zero value(as appropriate) in order to
    /// indicate that the message is a retransmission.
    /// The number of repeats should be set to either 1 or 2, indicating the number of further repeats required.
    /// All repeaters within coverage of one another should be set to the same number of repeats.
    /// Each time a received message is processed by the repeater station, the repeat indicator value should 
    /// be incremented by one(1) before retransmitting the message.If the processed repeat indicator equals 3
    /// the relevant message should not be retransmitted.
    /// <code>
    /// ╒═══════════════════════════════════════════╤═══════════╕
    /// │ Name                                      │ Value     │
    /// ╞═══════════════════════════════════════════╪═══════════╡
    /// │ Default                                   │     0     │
    /// ├───────────────────────────────────────────┼───────────┤
    /// │ RepeatedOnce                              │     1     │
    /// ├───────────────────────────────────────────┼───────────┤
    /// │ RepeatedTwice                             │     2     │
    /// ├───────────────────────────────────────────┼───────────┤
    /// │ NoRepeatAnymore                           │     3     │
    /// └───────────────────────────────────────────┴───────────┘
    /// </code>
    /// </summary>
    public enum RepeatIndicator: byte
    {
        /// <summary>
        /// Default
        /// </summary>
        Default = 0,

        /// <summary>
        /// Repeated once
        /// </summary>
        RepeatedOnce = 1,

        /// <summary>
        /// Repeated twice
        /// </summary>
        RepeatedTwice = 2,

        /// <summary>
        /// No repeat anymore
        /// </summary>
        NoRepeatAnymore = 3,
    }
}