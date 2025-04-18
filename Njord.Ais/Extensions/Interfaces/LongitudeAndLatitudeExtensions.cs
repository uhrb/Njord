using Njord.Ais.Interfaces;

namespace Njord.Ais.Extensions.Interfaces
{
    public static class LongitudeAndLatitudeExtensions
    {
        public const double LatitudeNotAvailable = 91;
        public const double LongitudeNotAvailable = 181;

        /// <summary>
        /// Checks if the latitude value is available.
        /// </summary>
        /// <param name="report">The position report.</param>
        /// <returns>True if latitude is available, otherwise false.</returns>
        public static bool IsLatitudeAvailiable(this ILongitudeAndLatitude report)
        {
            return report.Latitude != LatitudeNotAvailable;
        }

        /// <summary>
        /// Checks if the longitude value is available.
        /// </summary>
        /// <param name="report">The position report.</param>
        /// <returns>True if longitude is available, otherwise false.</returns>
        public static bool IsLongitudeAvailiable(this ILongitudeAndLatitude report)
        {
            return report.Longitude != LongitudeNotAvailable;
        }

        /// <summary>
        /// Checks if long and lat are in valid ranges
        /// </summary>
        /// <param name="report">report to check</param>
        /// <returns>True if in valid ranges</returns>
        public static bool IsLatitudeAndLongitudeValidRange(this ILongitudeAndLatitude report)
        {
            return report.Latitude >= -90 && report.Latitude <= 91 &&
                report.Longitude >= -180 && report.Longitude <= 181;
        }

    }
}
