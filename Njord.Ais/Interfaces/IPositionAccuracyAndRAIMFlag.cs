using Njord.Ais.Enums;

namespace Njord.Ais.Interfaces
{
    public interface IPositionAccuracyAndRAIMFlag
    {
        /// <summary>
        /// <para>
        /// The position accuracy (PA) flag should be determined in accordance with table
        /// 1 = high(≤ 10 m)
        /// 0 = low(>10 m)
        /// 0 = default
        /// </para>
        /// <code>
        /// ╒═══════════════════════════════════╤═══════════╤═══════════════════════════════════╤════════════════════════════╕
        /// │ Accuracy status from RAIM         │ RAIM flag │ Differential correction status(2) │ Resulting value of PA flag │
        /// │ (for 95% of position fixes)(1)    │           │                                   │                            │
        /// ╞═══════════════════════════════════╪═══════════╪═══════════════════════════════════╪════════════════════════════╡
        /// │ No RAIM process available         │     0     │                                   │ 0 = Low                    │
        /// ├───────────────────────────────────┼───────────┤                                   ├────────────────────────────┤
        /// │ EXPECTED RAIM error is ≤ 10 m     │     1     │           UNCORRECTED             │ 1 = High                   │
        /// ├───────────────────────────────────┼───────────┤                                   ├────────────────────────────┤
        /// │ EXPECTED RAIM error is > 10 m     │     1     │                                   │ 0 = Low                    │
        /// ├───────────────────────────────────┼───────────┼───────────────────────────────────┼────────────────────────────┤
        /// │ No RAIM process available         │     0     │                                   │ 1 = High                   │
        /// ├───────────────────────────────────┼───────────┤                                   ├────────────────────────────┤
        /// │ EXPECTED RAIM error is ≤ 10 m     │     1     │           CORRECTED               │ 1 = High                   │
        /// ├───────────────────────────────────┼───────────┤                                   ├────────────────────────────┤
        /// │ EXPECTED RAIM error is > 10 m     │     1     │                                   │ 0 = High                   │
        /// └───────────────────────────────────┴───────────┴───────────────────────────────────┴────────────────────────────┘
        /// </code>
        /// <para>
        /// 1) The connected GNSS receiver indicates the availability of a RAIM process by a valid sentence of IEC 61162; 
        /// in this case the RAIM-flag should be set to “1”. The threshold for evaluation of the RAIM information is 10 m. 
        /// The RAIM expected error is calculated based on “expected error in latitude” and “expected error in longitude” 
        /// </para>
        /// <para>
        /// 2) The quality indicator in the position sentences of IEC 61162 received from the connected GNSS receiver indicates the correction status.
        /// </para>
        /// </summary>
        public bool IsPositionAccuracyHigh { get; init; }

        /// <summary>
        /// <para>
        /// Receiver autonomous integrity monitoring (RAIM) flag of electronic position fixing device; 
        /// 0 = RAIM not in use = default; 1 = RAIM in use. 
        /// </para>
        ///  <code>
        /// ╒═══════════════════════════════════╤═══════════╤═══════════════════════════════════╤════════════════════════════╕
        /// │ Accuracy status from RAIM         │ RAIM flag │ Differential correction status(2) │ Resulting value of PA flag │
        /// │ (for 95% of position fixes)(1)    │           │                                   │                            │
        /// ╞═══════════════════════════════════╪═══════════╪═══════════════════════════════════╪════════════════════════════╡
        /// │ No RAIM process available         │     0     │                                   │ 0 = Low                    │
        /// ├───────────────────────────────────┼───────────┤                                   ├────────────────────────────┤
        /// │ EXPECTED RAIM error is ≤ 10 m     │     1     │           UNCORRECTED             │ 1 = High                   │
        /// ├───────────────────────────────────┼───────────┤                                   ├────────────────────────────┤
        /// │ EXPECTED RAIM error is > 10 m     │     1     │                                   │ 0 = Low                    │
        /// ├───────────────────────────────────┼───────────┼───────────────────────────────────┼────────────────────────────┤
        /// │ No RAIM process available         │     0     │                                   │ 1 = High                   │
        /// ├───────────────────────────────────┼───────────┤                                   ├────────────────────────────┤
        /// │ EXPECTED RAIM error is ≤ 10 m     │     1     │           CORRECTED               │ 1 = High                   │
        /// ├───────────────────────────────────┼───────────┤                                   ├────────────────────────────┤
        /// │ EXPECTED RAIM error is > 10 m     │     1     │                                   │ 0 = High                   │
        /// └───────────────────────────────────┴───────────┴───────────────────────────────────┴────────────────────────────┘
        /// </code>
        /// <para>
        /// 1) The connected GNSS receiver indicates the availability of a RAIM process by a valid sentence of IEC 61162; 
        /// in this case the RAIM-flag should be set to “1”. The threshold for evaluation of the RAIM information is 10 m. 
        /// The RAIM expected error is calculated based on “expected error in latitude” and “expected error in longitude” 
        /// </para>
        /// <para>
        /// 2) The quality indicator in the position sentences of IEC 61162 received from the connected GNSS receiver indicates the correction status.
        /// </para>
        /// </summary>
        public bool IsRaimInUse { get; init; }
    }
}
