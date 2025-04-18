namespace Njord.Ais.Enums
{
    /// <summary>
    /// Represents the types of aids to navigation used in maritime navigation.
    /// </summary>
    public enum AidsToNavigationType : byte
    {
        /// <summary>
        /// Type not specified.
        /// </summary>
        NotSpecified = 0,

        /// <summary>
        /// A reference point used for navigation.
        /// </summary>
        ReferencePoint = 1,

        /// <summary>
        /// Radar beacon (RACON) used for navigation.
        /// </summary>
        RACON = 2,

        /// <summary>
        /// Fixed structure offshore used for navigation.
        /// </summary>
        FixedStructureOffshore = 3,

        /// <summary>
        /// Buoy marking an emergency wreck.
        /// </summary>
        EmergencyWreckMarkingBuoy = 4,

        /// <summary>
        /// Fixed aid to navigation light without sectors.
        /// </summary>
        FixedAtoNLightWithoutSectors = 5,

        /// <summary>
        /// Fixed aid to navigation light with sectors.
        /// </summary>
        FixedAtoNLightWithSectors = 6,

        /// <summary>
        /// Fixed aid to navigation leading light (front).
        /// </summary>
        FixedAtoNLeadingLightFront = 7,

        /// <summary>
        /// Fixed aid to navigation leading light (rear).
        /// </summary>
        FixedAtoNLeadingLightRear = 8,

        /// <summary>
        /// Fixed aid to navigation beacon, cardinal north.
        /// </summary>
        FixedAtoNBeaconCardinalN = 9,

        /// <summary>
        /// Fixed aid to navigation beacon, cardinal east.
        /// </summary>
        FixedAtoNBeaconCardinalE = 10,

        /// <summary>
        /// Fixed aid to navigation beacon, cardinal south.
        /// </summary>
        FixedAtoNBeaconCardinalS = 11,

        /// <summary>
        /// Fixed aid to navigation beacon, cardinal west.
        /// </summary>
        FixedAtoNBeaconCardinalW = 12,

        /// <summary>
        /// Fixed aid to navigation beacon, port hand.
        /// </summary>
        FixedAtoNBeaconPortHand = 13,

        /// <summary>
        /// Fixed aid to navigation beacon, starboard hand.
        /// </summary>
        FixedAtoNBeaconStarboardHand = 14,

        /// <summary>
        /// Fixed aid to navigation beacon, preferred channel port hand.
        /// </summary>
        FixedAtoNBeaconPreferredChannelPortHand = 15,

        /// <summary>
        /// Fixed aid to navigation beacon, preferred channel starboard hand.
        /// </summary>
        FixedAtoNBeaconPreferredChannelStarboardHand = 16,

        /// <summary>
        /// Fixed aid to navigation beacon, isolated danger.
        /// </summary>
        FixedAtoNBeaconIsolatedDanger = 17,

        /// <summary>
        /// Fixed aid to navigation beacon, safe water.
        /// </summary>
        FixedAtoNBeaconSafeWater = 18,

        /// <summary>
        /// Fixed aid to navigation beacon, special mark.
        /// </summary>
        FixedAtoNBeaconSpecialMark = 19,

        /// <summary>
        /// Floating aid to navigation, cardinal mark north.
        /// </summary>
        FloatingAtoNCardinalMarkN = 20,

        /// <summary>
        /// Floating aid to navigation, cardinal mark east.
        /// </summary>
        FloatingAtoNCardinalMarkE = 21,

        /// <summary>
        /// Floating aid to navigation, cardinal mark south.
        /// </summary>
        FloatingAtoNCardinalMarkS = 22,

        /// <summary>
        /// Floating aid to navigation, cardinal mark west.
        /// </summary>
        FloatingAtoNCardinalMarkW = 23,

        /// <summary>
        /// Floating aid to navigation, port hand mark.
        /// </summary>
        FloatingAtoNPortHandMark = 24,

        /// <summary>
        /// Floating aid to navigation, starboard hand mark.
        /// </summary>
        FloatingAtoNStarboardHandMark = 25,

        /// <summary>
        /// Floating aid to navigation, preferred channel port hand.
        /// </summary>
        FloatingAtoNPreferredChannelPortHand = 26,

        /// <summary>
        /// Floating aid to navigation, preferred channel starboard hand.
        /// </summary>
        FloatingAtoNPreferredChannelStarboardHand = 27,

        /// <summary>
        /// Floating aid to navigation, isolated danger.
        /// </summary>
        FloatingAtoNIsolatedDanger = 28,

        /// <summary>
        /// Floating aid to navigation, safe water.
        /// </summary>
        FloatingAtoNSafeWater = 29,

        /// <summary>
        /// Floating aid to navigation, special mark.
        /// </summary>
        FloatingAtoNSpecialMark = 30,

        /// <summary>
        /// Floating aid to navigation, light vessel or LANBY or rigs.
        /// </summary>
        FloatingAtoNLightVesselOrLANBYOrRigs = 31,
    }
}
