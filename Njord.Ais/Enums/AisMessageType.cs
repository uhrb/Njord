namespace Njord.Ais.Enums
{
    /// <summary>
    /// Represents the different types of AIS (Automatic Identification System) messages.
    /// AIS messages are used to exchange vital navigational and vessel information between ships and monitoring stations.
    /// Each value corresponds to a specific message format and purpose, enabling vessels to communicate their position, status, and other critical navigational details.
    /// </summary>
    public enum AisMessageType : byte
    {
        /// <summary>
        /// Unknown message type.
        /// This value represents an unrecognized or unsupported AIS message format.
        /// </summary>
        UnknownMessage = 0,
        /// <summary>
        /// The position report should be output periodically by mobile stations.
        /// Typically used by large vessels, this scheduled message provides current navigational data such as position and speed.
        /// </summary>
        PositionReportScheduled = 1,

        /// <summary>
        /// The position report should be output periodically by mobile stations.
        /// An alternative encoding of a Class A scheduled position report assigned scheduled.
        /// </summary>
        PositionReportAssignedScheduled = 2,

        /// <summary>
        /// The position report should be used only by mobile stations, answering to Interrogation
        /// Used as answer for interrogation
        /// </summary>
        PositionReportSpecial = 3,

        /// <summary>
        /// Base station report from a fixed (static) base station.
        /// T. A base station should use Message 4 in its periodical transmissions. 
        /// Message 4 is used by AIS stations for determining if it is within 120 NM 
        /// for response to Messages 20 and 23. 
        /// </summary>
        BaseStationReport = 4,

        /// <summary>
        /// Static and voyage related data for a ship.
        /// Transmits non-dynamic, static information such as vessel identity, dimensions, and voyage particulars.
        /// </summary>
        ShipStaticAndVoyageRelatedData = 5,

        /// <summary>
        /// Addressed binary message.
        /// Utilized for exchange of binary data where the message is intended for a specific vessel or recipient.
        /// </summary>
        AddressedBinaryMessage = 6,

        /// <summary>
        /// Binary acknowledge message.
        /// A generic binary message used to acknowledge receipt of previous binary transmissions.
        /// </summary>
        BinaryAcknowledge = 7,

        /// <summary>
        /// Binary broadcast message
        /// </summary>
        BinaryBroadcast = 8,

        /// <summary>
        /// Position report for search and rescue aircraft.
        /// Tailored for aircraft involved in search and rescue operations to provide real-time positioning.
        /// </summary>
        StandardSearchAndRescueAircraftReport = 9,

        /// <summary>
        /// Coordinated Universal Time (UTC) and date inquiry.
        /// This type enables vessels to query or broadcast the current time and date, ensuring synchronized operations.
        /// </summary>
        CoordinatedUTCInquiry = 10,

        /// <summary>
        /// Base station report from a mobile station.
        /// A mobile station should output Message 11 only in response to interrogation by Message 10. 
        /// Message 11 is only transmitted as a result of a UTC request message (Message 10). The UTC and
        /// date response should be transmitted on the channel, where the UTC request message was received. 
        /// </summary>
        CoordinatedUtcBaseStationReport = 11,

        /// <summary>
        /// Addressed safety-related message.
        /// This message is directed towards a specific vessel with safety-related information.
        /// </summary>
        AddressedSafetyMessage = 12,

        /// <summary>
        /// Binary acknowledge for safety related message.
        /// Specifically acknowledges receipt of safety-related binary transmissions.
        /// </summary>
        BinaryAcknowledgeSafety = 13,

        /// <summary>
        /// Safety-related broadcast message.
        /// Broadcasts general safety information to all vessels within range.
        /// </summary>
        SafetyBroadcastMessage = 14,

        /// <summary>
        /// Interrogation message.
        /// Used by vessels or stations to request information from other AIS stations.
        /// </summary>
        Interrogation = 15,

        /// <summary>
        /// Assigned mode command.
        /// Used to issue specific commands to a vessel’s AIS system, often related to operational modes.
        /// </summary>
        AssignedModeCommand = 16,

        /// <summary>
        /// GNSS (Global Navigation Satellite System) broadcast binary message.
        /// Broadcasts binary data derived from satellite navigation systems.
        /// </summary>
        GnssBroadcastBinaryMessage = 17,

        /// <summary>
        /// Standard position report for Class B shipborne mobile equipment.
        /// Transmits basic navigational data for vessels using Class B equipment.
        /// </summary>
        StandardClassBPositionReport = 18,

        /// <summary>
        /// Extended position report for Class B shipborne mobile equipment.
        /// Provides enhanced navigational information for smaller or non-Class A vessels.
        /// </summary>
        ExtendedClassBPositionReport = 19,

        /// <summary>
        /// Data link management message.
        /// Manages the data link layer communication ensuring proper data transfer protocols.
        /// </summary>
        DataLinkManagementMessage = 20,

        /// <summary>
        /// Aids to navigation report.
        /// Provides information pertaining to navigational aids like buoys and beacons.
        /// </summary>
        AidsToNavigationReport = 21,

        /// <summary>
        /// Channel management message.
        /// Used to coordinate and manage communication channels among vessels and shore stations.
        /// </summary>
        ChannelManagement = 22,

        /// <summary>
        /// Group assignment command
        /// </summary>
        GroupAssignmentCommand = 23,

        /// <summary>
        /// Static data report.
        /// Provides fixed or slowly changing information about a vessel, useful for identification and classification.
        /// </summary>
        StaticDataReport = 24,

        /// <summary>
        /// Binary message with a single slot.
        /// Contains binary data in one time slot, typically used for simpler data transmissions.
        /// </summary>
        SingleSlotBinaryMessage = 25,

        /// <summary>
        /// Binary message with multiple slots.
        /// Contains binary data organized into several time slots, useful for complex data transmissions.
        /// </summary>
        MultiSlotBinaryMessage = 26,

        /// <summary>
        /// Long-range AIS broadcast message.
        /// Optimized for long-distance communications, providing extended range reporting.
        /// </summary>
        LongRangeAisBroadcastMessage = 27,
    }
}
