using Njord.Ais.Interfaces;

namespace Njord.Ais.Messages
{
    /// <summary>
    /// GNSS broadcast binary message. For Unification, SourceID from original message is going to UserId of the message.
    /// </summary>
    public interface IGnssBroadcastBinaryMessage : IUserId, IMessageId, IRepeatIndicator, ILongitudeAndLatitude
    {
        /// <summary>
        /// Differential correction data. If interrogated and differential
        ///  correction service not available, the data field should remain empty(zero
        /// bits). This should be interpreted by the recipient as DGNSS data words set to zero
        /// </summary>
        public ReadOnlyMemory<byte> DifferentialCorrectionData { get; init; }
    }
}
