using Njord.Ais.Interfaces;

namespace Njord.Ais.Extensions.Interfaces
{
    public static class SpeedAndCourseOverGroundExtensions
    {
        public const double CourseNotAvailable = 360;

        public const double SpeedNotAvailable = 102.3;

        /// <summary>
        /// Checks if the course over ground value is available.
        /// </summary>
        /// <param name="report">The position report.</param>
        /// <returns>True if course over ground is available, otherwise false.</returns>
        public static bool IsCourseOverGroundAvailiable(this ISpeedAndCourseOverGround report)
        {
            return report.CourseOverGround != CourseNotAvailable;
        }

        /// <summary>
        /// Checks if the speed over ground value is available.
        /// </summary>
        /// <param name="report">The position report.</param>
        /// <returns>True if speed over ground is available, otherwise false.</returns>
        public static bool IsSpeedOverGroundAvailiable(this ISpeedAndCourseOverGround report)
        {
            return report.SpeedOverGround != SpeedNotAvailable;
        }

        public static bool IsSpeedOverGroundValidRange(this ISpeedAndCourseOverGround report)
        {
            return report.SpeedOverGround >= 0 && report.SpeedOverGround <= 102.3;
        }

        public static bool IsCourseOverGroundValidRange(this ISpeedAndCourseOverGround report)
        {
            return report.CourseOverGround >= 0 && report.CourseOverGround <= 360;
        }
    }
}
