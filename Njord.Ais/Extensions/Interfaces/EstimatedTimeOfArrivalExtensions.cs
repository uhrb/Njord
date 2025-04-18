using Njord.Ais.Interfaces;

namespace Njord.Ais.Extensions.Interfaces
{
    public static class EstimatedTimeOfArrivalExtensions
    {
        /// <summary>
        /// Checks validity of ETA
        /// </summary>
        /// <param name="eta">ETA</param>
        /// <returns>True if availiable</returns>
        public static bool IsETAAvailiable(this IEstimatedTimeOfArrival eta)
        {
            return !(eta.Month == 0 && eta.Day == 0 && eta.Hour == 24 && eta.Minute == 60);
        }

        /// <summary>
        /// Converts ETA to timespan
        /// </summary>
        /// <param name="eta">ETA</param>
        /// <returns>DateTime or null of cannot be calculated or not availiable</returns>
        public static DateTime? ToDateTime(this IEstimatedTimeOfArrival eta, ushort year)
        {
            if (!eta.IsETAAvailiable())
            {
                return null;
            }

            return new DateTime(year, eta.Month, eta.Day, eta.Hour, eta.Minute, 0, DateTimeKind.Utc);
        }
    }
}
