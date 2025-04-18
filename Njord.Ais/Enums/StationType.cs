namespace Njord.Ais.Enums
{
    /// <summary>
    /// Station type
    /// </summary>
    public enum StationType : byte
    {
        /// <summary>
        /// All types of mobile stations
        /// </summary>
        AllTypesOfMobileStations = 0,

        /// <summary>
        /// Class A mobile station only
        /// </summary>
        ClassAMobileStationOnly = 1,

        /// <summary>
        /// All types of Class B mobile stations
        /// </summary>
        AllTypesClassBMobileStations = 2,

        /// <summary>
        /// Search and Rescue (SAR) airborne mobile station
        /// </summary>
        SARAirborneMobileStation = 3,

        /// <summary>
        /// Class B mobile stations using Self-Organizing Time Division Multiple Access (SOTDMA)
        /// </summary>
        ClassBMobileStationsSO = 4,

        /// <summary>
        /// Class B mobile stations using Carrier-Sense Time Division Multiple Access (CSTDMA)
        /// </summary>
        ClassBMobileStationsCS = 5,

        /// <summary>
        /// Inland waterways mobile stations
        /// </summary>
        InlandWaterwaysMobileStations = 6,

        /// <summary>
        /// Regional use 0
        /// </summary>
        RegionalUse0 = 7,

        /// <summary>
        /// Regional use 1
        /// </summary>
        RegionalUse1 = 8,

        /// <summary>
        /// Regional use 2
        /// </summary>
        RegionalUse2 = 9,

        /// <summary>
        /// Base station coverage area
        /// </summary>
        BaseStationCoverageArea = 10,

        /// <summary>
        /// Reserved for future use 0
        /// </summary>
        Reserved0 = 11,

        /// <summary>
        /// Reserved for future use 1
        /// </summary>
        Reserved1 = 12,

        /// <summary>
        /// Reserved for future use 2
        /// </summary>
        Reserved2 = 13,

        /// <summary>
        /// Reserved for future use 3
        /// </summary>
        Reserved3 = 14,

        /// <summary>
        /// Reserved for future use 4
        /// </summary>
        Reserved4 = 15
    }
}
