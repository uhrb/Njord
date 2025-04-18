using Njord.Ais.Interfaces;

namespace Njord.Ais.Extensions.Interfaces
{
    public static class TimestampExtensions
    {

        /// <summary>
        /// Checks if the timestamp value is valid.
        /// </summary>
        /// <param name="report">The position report.</param>
        /// <returns>True if timestamp is valid, otherwise false.</returns>
        public static bool IsTimestampValid(this ITimestamp report)
        {
            return report.Timestamp <= 59;
        }

        /// <summary>
        /// Checks if the timestamp value is valid time.
        /// </summary>
        /// <param name="report">The position report.</param>
        /// <returns>True if timestamp is valid time, otherwise false.</returns>
        public static bool IsTimestampValidTime(this ITimestamp report)
        {
            return report.Timestamp <= 63;
        }

        /// <summary>
        /// Checks if the timestamp value indicates not available.
        /// </summary>
        /// <param name="report">The position report.</param>
        /// <returns>True if timestamp indicates not available, otherwise false.</returns>
        public static bool IsTimestampNotAvaiable(this ITimestamp report)
        {
            return report.Timestamp == 60;
        }

        /// <summary>
        /// Checks if the timestamp value indicates manual mode.
        /// </summary>
        /// <param name="report">The position report.</param>
        /// <returns>True if timestamp indicates manual mode, otherwise false.</returns>
        public static bool IsTimestampManualMode(this ITimestamp report)
        {
            return report.Timestamp == 61;
        }

        /// <summary>
        /// Checks if the timestamp value indicates estimated mode.
        /// </summary>
        /// <param name="report">The position report.</param>
        /// <returns>True if timestamp indicates estimated mode, otherwise false.</returns>
        public static bool IsTimestampEstimatedMode(this ITimestamp report)
        {
            return report.Timestamp == 62;
        }

        /// <summary>
        /// Checks if the timestamp value indicates inoperable mode.
        /// </summary>
        /// <param name="report">The position report.</param>
        /// <returns>True if timestamp indicates inoperable mode, otherwise false.</returns>
        public static bool IsTimestampInoperableMode(this ITimestamp report)
        {
            return report.Timestamp == 63;
        }
    }
}
