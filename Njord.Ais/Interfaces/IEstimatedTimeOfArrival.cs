namespace Njord.Ais.Interfaces
{
    /// <summary>
    /// Estimated time of arrival
    /// Estimated time of arrival; MMDDHHMM UTC
    ///  <code>
    /// ╒═══════════════╤══════════════════════════════════════════════╕
    /// │ Bits          │ Description                                  │
    /// ╞═══════════════╪══════════════════════════════════════════════╡
    /// │     19-16     │ month 1-12; 0 = not available = default      │ 
    /// ├───────────────┼──────────────────────────────────────────────┤
    /// │     15-11     │  day; 1-31; 0 = not available = default      │ 
    /// ├───────────────┼──────────────────────────────────────────────┤
    /// │     10-6      │ hour; 0-23; 24 = not available = default     │
    /// ├───────────────┼──────────────────────────────────────────────┤
    /// │     5-0       │ minute; 0-59; 60 = not available = default   │
    /// └───────────────┴──────────────────────────────────────────────┘
    /// </code>
    ///Bits 19-16: month; 1-12; 0 = not available = default
    ///Bits 15-11: day; 1-31; 0 = not available = default
    ///Bits 10-6: hour; 0-23; 24 = not available = default
    ///Bits 5-0: minute; 0-59; 60 = not available = default
    ///For SAR aircraft, the use of this field may be decided by the responsible
    ///administration
    /// </summary>
    public interface IEstimatedTimeOfArrival
    {
        /// <summary>
        /// Month; 1-12; 0 = not available = default
        /// </summary>
        public byte Month { get; init; }

        /// <summary>
        /// Day; 1-31; 0 = not available = default
        /// </summary>
        public byte Day { get; init; }

        /// <summary>
        /// Hour; 0-23; 24 = not available = default
        /// </summary>
        public byte Hour { get; init; }

        /// <summary>
        /// Minute; 0-59; 60 = not available = default
        /// </summary>
        public byte Minute { get; init; }
    }
}
