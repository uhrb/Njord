using Njord.Ais.Enums;
using Njord.Ais.Interfaces;

namespace Njord.Ais.Messages
{
    public interface IBaseStationReportMessage : IMessageId, IUserId, IPositionWithAccuracy, ICommunicationState, IPositionFixingDeviceInfo
    {
        /// <summary>
        /// 1-9999; 0 = UTC year not available = default
        /// </summary>
        public ushort UTCYear { get; init; }

        /// <summary>
        /// 1-12; 0 = UTC month not available = default; 13-15 not used 
        /// </summary>
        public byte UTCMonth { get; init; }

        /// <summary>
        /// 1-31; 0 = UTC day not available = default 
        /// </summary>
        public byte UTCDay { get; init; }

        /// <summary>
        /// 0-23; 24 = UTC hour not available = default; 25-31 not used 
        /// </summary>
        public byte UTCHour { get; init; }

        /// <summary>
        /// 0-59; 60 = UTC minute not available = default; 61-63 not used 
        /// </summary>
        public byte UTCMinute { get; init; }

        /// <summary>
        /// 0-59; 60 = UTC second not available = default; 61-63 not used
        /// </summary>
        public byte UTCSecond { get; init; }

        /// <summary>
        /// Long range control. Request Class-A station to transmit Message 27 within an AIS base station coverage area.
        /// </summary>
        public bool AskLongRangeRetransmission { get; init; }
    }
}
