using Njord.Ais.Enums;
using Njord.Ais.Extensions.Interfaces;
using Njord.Ais.Extensions.Types;
using Njord.Ais.Messages;

namespace Njord.Ais.Extensions.Messages
{
    public static class StandardSARAircraftPositionReportMessageExtenstions
    {
        public static bool IsValid(this IStandardSARAircraftPositionReportMessage message)
        {
            return message.UserId.IsValidMMSI()
                && message.MessageId == AisMessageType.StandardSearchAndRescueAircraftReport
                && message.IsCourseOverGroundValidRange()
                // not checking SOG. Sometimes its very high and its okay.
                && message.IsLatitudeAndLongitudeValidRange()
                && message.IsAltitudeValidRange();
        }

        public static bool IsAltitudeValidRange(this IStandardSARAircraftPositionReportMessage message)
        {
            return message.Altitude >= 0 && message.Altitude <= 4095;
        }

        public static bool IsAltitudeUnavailiable(this IStandardSARAircraftPositionReportMessage message)
        {
            return message.Altitude == 4095;
        }

        public static bool IsAlitudeHigherThanMaxForReport(this IStandardSARAircraftPositionReportMessage message)
        {
            return message.Altitude == 4094;
        }
    }
}
