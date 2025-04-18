using Njord.Ais.Interfaces;

namespace Njord.Ais.Extensions.Interfaces
{
    public static class TrueHeadingExtensions
    {
        public const int TrueHeadingNotAvailable = 511;

        /// <summary>
        /// Checks if the true heading value is available.
        /// </summary>
        /// <param name="report">The position report.</param>
        /// <returns>True if true heading is available, otherwise false.</returns>
        public static bool IsTrueHeadingAvailiable(this ITrueHeading report)
        {
            return report.TrueHeading != TrueHeadingNotAvailable;
        }

        public static bool IsTrueHeadingValidRange(this ITrueHeading report)
        {
            return report.TrueHeading >= 0 && report.TrueHeading < 360 || report.TrueHeading == 511;
        }
    }
}
