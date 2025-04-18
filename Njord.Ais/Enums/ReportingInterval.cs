namespace Njord.Ais.Enums
{
    /// <summary>
    /// Enum representing the reporting intervals for AIS (Automatic Identification System).
    /// </summary>
    public enum ReportingInterval : byte
    {
        /// <summary>
        /// Reporting interval as given in autonomous mode.
        /// </summary>
        AsGivenInAutonomousMode = 0,

        /// <summary>
        /// Reporting interval of 10 minutes.
        /// </summary>
        Minutes10 = 1,

        /// <summary>
        /// Reporting interval of 6 minutes.
        /// </summary>
        Minutes6 = 2,

        /// <summary>
        /// Reporting interval of 3 minutes.
        /// </summary>
        Minutes3 = 3,

        /// <summary>
        /// Reporting interval of 1 minute.
        /// </summary>
        Minutes1 = 4,

        /// <summary>
        /// Reporting interval of 30 seconds.
        /// </summary>
        Seconds30 = 5,

        /// <summary>
        /// Reporting interval of 15 seconds.
        /// </summary>
        Seconds15 = 6,

        /// <summary>
        /// Reporting interval of 10 seconds.
        /// </summary>
        Seconds10 = 7,

        /// <summary>
        /// Reporting interval of 5 seconds.
        /// </summary>
        Seconds5 = 8,

        /// <summary>
        /// Next shorter reporting interval.
        /// </summary>
        NextShorterReportingInterval = 9,

        /// <summary>
        /// Next longer reporting interval.
        /// </summary>
        NextLongerReportingInterval = 10,

        /// <summary>
        /// Reporting interval of 2 seconds.
        /// </summary>
        Seconds2 = 11,

        /// <summary>
        /// Reserved for future use.
        /// </summary>
        Reserved0 = 12,

        /// <summary>
        /// Reserved for future use.
        /// </summary>
        Reserved1 = 13,

        /// <summary>
        /// Reserved for future use.
        /// </summary>
        Reserved2 = 14,

        /// <summary>
        /// Reserved for future use.
        /// </summary>
        Reserved3 = 15,
    }
}
