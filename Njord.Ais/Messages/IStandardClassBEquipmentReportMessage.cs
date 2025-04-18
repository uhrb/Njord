using Njord.Ais.Enums;
using Njord.Ais.Interfaces;

namespace Njord.Ais.Messages
{
    public interface IStandardClassBEquipmentReportMessage :
        IUserId,
        IMessageId,
        IRepeatIndicator, 
        ITimestamp, 
        ICommunicationState, 
        ICommunicationStateSelector, 
        IAssignedModeFlag, 
        ISpeedAndCourseOverGround, 
        ILongitudeAndLatitude, 
        IPositionAccuracyAndRAIMFlag
    {

        /// <summary>
        /// Class B unit flag. True if unit is CS, otherwise unit is SOTDMA
        /// </summary>
        public bool IsCSUnit { get; init; }

        /// <summary>
        /// Class B display flag. True if integrated display availiable and able to show messages 14 and 19
        /// </summary>
        public bool IsDisplayAvailiable { get; init; }

        /// <summary>
        /// Class B DSC flag. True if distress call system equipment is availiable
        /// </summary>
        public bool IsDSCEquipmentAvailiable { get; init; }

        /// <summary>
        /// Class B channel management flag. True if channel management is supported
        /// </summary>
        public bool IsSupportChannelManagement { get; init; }

        /// <summary>
        /// 0 = Capable of operating over the upper 525 kHz band of the marine band 
        /// 1 = Capable of operating over the whole marine band (irrelevant if “Class B Message 22 flag” is 0)
        /// </summary>
        public bool IsCapableToOperateOverWholeMarineBand { get; init; }
    }
}
