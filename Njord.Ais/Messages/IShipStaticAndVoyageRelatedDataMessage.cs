using Njord.Ais.Enums;
using Njord.Ais.Interfaces;

namespace Njord.Ais.Messages
{
    public interface IShipStaticAndVoyageRelatedDataMessage : IUserId, IMessageId, IRepeatIndicator, ITypeOfShipAndCargo, IDimensionsProvided, IPositionFixingDeviceInfo, IDataTerminalEquipment, IName, ICallSign
    {
        /// <summary>
        /// Ais version indicator
        /// </summary>
        public  AisVersion AisVersion { get; init; }

        /// <summary>
        /// <code>
        /// ╒═══════════════════════════════════════════╤═══════════════════════╕
        /// │ Name                                      │ Value                 │
        /// ╞═══════════════════════════════════════════╪═══════════════════════╡
        /// │ Default                                   │ 0                     │
        /// ├───────────────────────────────────────────┼───────────────────────┤
        /// │ Not used                                  │ 0000000001-0000999999 │
        /// ├───────────────────────────────────────────┼───────────────────────┤
        /// │ Valid number                              │ 0001000000-0009999999 │
        /// ├───────────────────────────────────────────┼───────────────────────┤
        /// │ official flag state number                │ 0010000000-1073741823 │
        /// └───────────────────────────────────────────┴───────────────────────┘
        /// </code>
        /// </summary>
        public  uint IMONumber { get; init; }

        /// <summary>
        /// Estimated time of arrival. See <see cref="IEstimatedTimeOfArrival"/>
        /// </summary>
        public  IEstimatedTimeOfArrival EstimatedTimeOfArrival { get; init; }

        /// <summary>
        /// For simplicity should be set in metric decimal. Originaly, In 1/10 m, 255 = draught 25.5 m or greater, 0 = not available = default;
        /// in accordance with IMO Resolution A.851
        /// Not applicable to SAR aircraft, should be set to 0
        /// </summary>
        public  float MaximumPresentStaticDraught { get; init; }

        /// <summary>
        /// Maximum 20 characters using 6-bit ASCII;
        /// @@@@@@@@@@@@@@@@@@@@ = not available
        /// For SAR aircraft, the use of this field may be decided by the responsible
        /// administration
        /// </summary>
        public  string Destination { get; init; }

    }
}
