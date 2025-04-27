namespace Njord.Ais.Enums
{
    /// <summary>
    /// Describes lights status of AtoN
    /// </summary>
    public enum AidsToNavigationLightsStatus : byte
    {
        /// <summary>
        /// Not equepped with lights or light equipment is not monitored
        /// </summary>
        NoLightsOrNoMonitoring = 0,

        /// <summary>
        /// Light is on
        /// </summary>
        LightOn = 1,

        /// <summary>
        /// Light is off
        /// </summary>
        LightOff = 2,

        /// <summary>
        /// Light mailfunction
        /// </summary>
        LightError = 3
    }
}
