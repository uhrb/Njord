using Njord.Ais.Enums;
using Njord.Ais.Interfaces;

namespace Njord.Ais.Messages
{
    /// <summary>
    /// This message should be transmitted by a base station (as a broadcast message) to command the VHF
    /// data link parameters for the geographical area designated in this message and should be accompanied
    /// by a Message 4 transmission for evaluation of the message within 120 NMhannel
    /// </summary>
    public interface IChannelManagementMessage : IUserId, IMessageId, IRepeatIndicator, ITxRxModeType
    {
        /// <summary>
        /// Channel number of 25 kHz simplex or simplex use of 25 kHz duplex in
        /// accordance with Recommendation ITU-R M.1084.
        /// </summary>
        public ushort ChannelA { get; init; }

        /// <summary>
        /// Channel number of 25 kHz simplex or simplex use of 25 kHz duplex in
        /// accordance with Recommendation ITU-R M.1084.
        /// </summary>
        public ushort ChannelB { get; init; }

        /// <summary>
        /// True, if low power type channel 
        /// </summary>
        public bool IsLowPower { get; init; }

        /// <summary>
        /// In case not IsAddressed contains information about area
        /// </summary>
        public IGeoArea? Area { get; init; }

        /// <summary>
        /// In case IsAddressed contains information about destination ids
        /// </summary>
        public IEnumerable<string>? AddressedStations { get; init; }

        /// <summary>
        /// True if message addressed, otherwise its broadcast
        /// </summary>
        public bool IsAddressed { get; init; }

        /// <summary>
        /// C0 = default (as specified by channel number); 1 = spare (formerly 12.5 kHz bandwidth in Recommendation ITU-R M.1371-1 now obsolete)
        /// </summary>
        public bool IsChannelABandwidthSpare { get; init; }

        /// <summary>
        /// 0 = default (as specified by channel number); 1 = spare (formerly 12.5 kHz bandwidth in Recommendation ITU-R M.1371-1 now obsolete)
        /// </summary>
        public bool IsChannelBBandwidthSpare { get; init; }

        /// <summary>
        /// The transitional zone size in nautical miles should be calculated by adding 1
        /// to this parameter value. The default parameter value should be 4, which
        /// translates to 5 nautical miles;
        /// </summary>
        public byte TransitionalZoneSize { get; init; }
    }
}
