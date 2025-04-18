using Njord.Ais.Enums;
using Njord.Ais.Interfaces;

namespace Njord.Ais.Messages
{
    public interface IPositionReportMessage : IUserId, 
        IMessageId, ITimestamp, ICommunicationState, ITrueHeading, INavigationalStatus, IMovingPosition
    {
        /// <summary>
        /// Values between 0 and 708° per min coded by ROTAIS = 4.733 SQRT(ROTsensor) 
        /// degrees per min where ROTsensor is the Rate of Turn as input by 
        /// an external Rate of Turn Indicator(TI). ROTAIS is rounded to the nearest integer value.
        ///  <code>
        /// ╒═════════════════════════════════════════════════╤═══════════╕
        /// │ Name                                            │ Value     │
        /// ╞═════════════════════════════════════════════════╪═══════════╡
        /// │ turning right at up to 708° per min or higher   │ 0 to +126 │
        /// ├─────────────────────────────────────────────────┼───────────┤
        /// │ turning left at up to 708° per min or higher    │ 0 to –126 │
        /// ├─────────────────────────────────────────────────┼───────────┤
        /// │ turning right at more than 5° per 30s           │           │
        /// │ (No TI available)                               │    +127   │
        /// ├─────────────────────────────────────────────────┼───────────┤
        /// │ turning left at more than 5° per 30s            │           │
        /// │ (No TI available)                               │    +128   │
        /// ├─────────────────────────────────────────────────┼───────────┤
        /// │ No turn information availiable                  │    -128   │  
        /// └─────────────────────────────────────────────────┴───────────┘
        /// </code>
        /// ROT data should not be derived from COG information.
        /// </summary>
        public sbyte RateOfTurn { get; init; }

        /// <summary>
        /// Special manoeuvre indicator. See <see cref="Enums.SpecialManoeuvreIndicator"/> for details.
        /// </summary>
        public SpecialManoeuvreIndicator SpecialManoeuvreIndicator { get; init; }

    }
}
