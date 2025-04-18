using Njord.Ais.Enums;
using Njord.Ais.Extensions.Interfaces;
using Njord.Ais.Extensions.Types;
using Njord.Ais.Messages;

namespace Njord.Ais.Extensions.Messages
{
    public static class StandardClassBEquipmentReportMessageExtensions
    {
        public static bool IsValid(this IStandardClassBEquipmentReportMessage message)
        {
            return message.UserId.IsValidMMSI()
                && message.IsCourseOverGroundValidRange()
                && message.IsLatitudeAndLongitudeValidRange()
                && message.IsSpeedOverGroundValidRange()
                && message.MessageId == AisMessageType.StandardClassBPositionReport;
        }
    }
}
