using Njord.Ais.Enums;
using Njord.Ais.Extensions.Interfaces;
using Njord.Ais.Extensions.Types;
using Njord.Ais.Messages;

namespace Njord.Ais.Extensions.Messages
{
    public static class ExtendedClassBEquipmentPositionReportMessageExtensions
    {
        public static bool IsValid(this IExtendedClassBEquipmentPositionReportMessage message)
        {
            return message.MessageId == AisMessageType.ExtendedClassBPositionReport
                && message.UserId.IsValidMMSI()
                && message.IsCourseOverGroundValidRange()
                && message.IsSpeedOverGroundValidRange()
                && message.IsLatitudeAndLongitudeValidRange()
                && message.IsTrueHeadingValidRange();
        }
    }
}
