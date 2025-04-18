using Njord.Ais.Enums;
using Njord.Ais.Interfaces;

namespace Njord.Ais.Messages
{
    /// <summary>
    /// The Group assignment command is transmitted by a base station when operating as a controlling
    /// entity. This message should be applied to a mobile station within
    /// the defined region and as selected by “Ship and Cargo Type” or “Station type”. The receiving station
    /// should consider all selector fields concurrently.It controls the following operating parameters of a
    /// mobile station:
    /// – transmit/receive mode;
    /// – reporting interval;
    /// – the duration of a quiet time.
    /// </summary>
    public interface IGroupAssignmentCommandMessage : IUserId, IMessageId, IRepeatIndicator, ITypeOfShipAndCargo, ITxRxModeType
    {
        public IGeoArea GeoArea { get; init; }

        /// <summary>
        /// Station type <see cref="Enums.StationType"/>
        /// </summary>
        public StationType StationType { get; init; }

        /// <summary>
        /// Reporting interval <see cref="Enums.ReportingInterval"/>
        /// </summary>
        public ReportingInterval ReportingInterval { get; init; }

        /// <summary>
        /// Quiet time commanded
        /// </summary>
        public byte QuietTime { get; init; }
    }
}
