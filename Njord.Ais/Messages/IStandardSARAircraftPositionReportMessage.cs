using Njord.Ais.Enums;
using Njord.Ais.Interfaces;

namespace Njord.Ais.Messages
{
    /// <summary>
    /// SAR aricraft position report
    /// </summary>
    public interface IStandardSARAircraftPositionReportMessage : IUserId, ITimestamp, IMessageId, ICommunicationState, ICommunicationStateSelector, IDataTerminalEquipment, IAssignedModeFlag, ISpeedAndCourseOverGround, ILongitudeAndLatitude, IPositionAccuracyAndRAIMFlag
    {
        /// <summary>
        /// Altitude (derived from GNSS or barometric (see altitude sensor parameter
        /// below)) (m) (0-4 094 m) 4 095 = not available, 4 094 = 4 094 m or higher
        /// </summary>
        public ushort Altitude { get; init; }

        /// <summary>
        /// Is Altitude sensor type barometric? Otherwise from GNSS
        /// </summary>
        public bool IsAltitudeSensorTypeBarometric { get; init; }
    }
}
