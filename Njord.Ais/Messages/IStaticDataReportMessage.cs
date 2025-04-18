using Njord.Ais.Interfaces;

namespace Njord.Ais.Messages
{
    /// <summary>
    /// Equipment that supports Message 24 part A shall transmit once every 6 min alternating between
    /// channels.
    /// Message 24 Part A may be used by any AIS station to associate a MMSI with a name.
    /// Message 24 Part A and Part B should be transmitted once every 6 min by Class B “CS” and Class B
    /// “SO” shipborne mobile equipment.The message consists of two parts. Message 24B should be
    /// transmitted within 1 min following Message 24A.
    /// When the parameter value of dimension of ship/reference for position or type of electronic position
    /// fixing device is changed, Class-B :CS” and Class-B “SO” should transmit Message 24B.
    /// When requesting the transmission of a Message 24 from a Class B “CS” or Class B “SO”, the AIS
    /// station should respond with part A and part B.
    /// When requesting the transmission of a Message 24 from a Class A, the AIS station should respond
    /// with part B, which may contain the vendor ID only.
    /// </summary>
    public interface IStaticDataReportMessage : IUserId, IMessageId, IRepeatIndicator, ITypeOfShipAndCargo, IDimensionsProvided, IPositionFixingDeviceInfo, ICallSign, IName
    {
        /// <summary>
        /// Identifies which part of the message is set. True if PartA, otherwise PartB
        /// </summary>
        public bool IsPartA { get; init; }

        /// <summary>
        /// Manufacturer ID of the ship's equipment.
        /// </summary>
        public string? ManufacturerId { get; init; }

        /// <summary>
        /// Unit model code of the ship's equipment.
        /// </summary>
        public byte? UnitModelCode { get; init; }

        /// <summary>
        /// Unit serial number of the ship's equipment.
        /// </summary>
        public uint? UnitSerialNumber { get; init; }
    }
}
