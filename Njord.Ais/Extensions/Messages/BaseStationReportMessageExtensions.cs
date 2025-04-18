using Njord.Ais.Extensions.Interfaces;
using Njord.Ais.Extensions.Types;
using Njord.Ais.Messages;

namespace Njord.Ais.Extensions.Messages
{
    /// <summary>
    /// Base station position reports
    /// </summary>
    public static class BaseStationReportMessageExtensions
    {
        /// <summary>
        /// Checks is year valid
        /// </summary>
        public static bool IsUTCYearAvailable(this IBaseStationReportMessage msg)
        {
            return msg.UTCYear > 0 && msg.UTCYear <= 9999;
        }

        /// <summary>
        /// Checks if month valid
        /// </summary>
        public static bool IsUTCMonthAvailable(this IBaseStationReportMessage msg)
        {
            return msg.UTCMonth > 0 && msg.UTCMonth <= 12;
        }

        /// <summary>
        /// Check if Day valid
        /// </summary>
        public static bool IsUTCDayAvailable(this IBaseStationReportMessage msg)
        {
            return msg.UTCDay > 0 && msg.UTCDay <= 31;
        }

        /// <summary>
        /// Check if hour valid
        /// </summary>
        public static bool IsUTCHourAvailiable(this IBaseStationReportMessage msg)
        {
            return msg.UTCHour >= 0 && msg.UTCHour <= 23;
        }

        /// <summary>
        /// Checks if minute valid
        /// </summary>
        public static bool IsUTCMinuteAvailiable(this IBaseStationReportMessage msg)
        {
            return msg.UTCMinute >= 0 && msg.UTCMinute <= 59;
        }

        /// <summary>
        /// Checks if second valid
        /// </summary>
        public static bool IsUTCSecondAvailiable(this IBaseStationReportMessage msg)
        {
            return msg.UTCSecond >= 0 && msg.UTCSecond <= 59;
        }

        /// <summary>
        /// Checks if date fully valid
        /// </summary>
        public static bool IsUTCDateAvailiable(this IBaseStationReportMessage msg)
        {
            return msg.IsUTCYearAvailable() && msg.IsUTCMonthAvailable() && msg.IsUTCDayAvailable() && msg.IsUTCHourAvailiable() && msg.IsUTCMinuteAvailiable() && msg.IsUTCSecondAvailiable();
        }

        public static bool IsValid(this IBaseStationReportMessage msg)
        {
            return msg.UserId.IsValidMMSI()
                && msg.IsLatitudeAndLongitudeValidRange()
                && msg.UTCYear <= 9999
                && msg.UTCMonth <= 12
                && msg.UTCDay <= 31
                && msg.UTCHour <= 24
                && msg.UTCMinute <= 60
                && msg.UTCSecond <= 60;
        }
    }
}
