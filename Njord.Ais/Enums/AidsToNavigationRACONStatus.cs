namespace Njord.Ais.Enums
{
    /// <summary>
    /// Describes status of AtoN RACON (Radar transponder beacon)
    /// </summary>
    public enum AidsToNavigationRACONStatus : byte
    {
        /// <summary>
        /// Not installed
        /// </summary>
        NoRACONInstalled = 0,

        /// <summary>
        /// Installed but not monitored
        /// </summary>
        RACONInstalledButNotMonitored = 1,

        /// <summary>
        /// Operational
        /// </summary>
        RACONOperational = 2,

        /// <summary>
        /// RACON Error
        /// </summary>
        RACONError = 3
    }
}
