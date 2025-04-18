using Njord.Ais.Interfaces;

namespace Njord.Ais.Messages
{
    public interface IExtendedClassBEquipmentPositionReportMessage :
        IUserId, 
        IMessageId,
        IRepeatIndicator, 
        ITimestamp, 
        ITrueHeading, 
        ITypeOfShipAndCargo, 
        IDimensionsProvided, 
        IPositionFixingDeviceInfo, 
        IDataTerminalEquipment, 
        IAssignedModeFlag, 
        IName, 
        ISpeedAndCourseOverGround, 
        ILongitudeAndLatitude, 
        IPositionAccuracyAndRAIMFlag
    {
    }
}
