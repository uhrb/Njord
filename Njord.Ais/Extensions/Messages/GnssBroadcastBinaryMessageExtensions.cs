using Njord.Ais.Enums;
using Njord.Ais.Extensions.Interfaces;
using Njord.Ais.Extensions.Types;
using Njord.Ais.Messages;

namespace Njord.Ais.Extensions.Messages
{
    public static class GnssBroadcastBinaryMessageExtensions
    {
        public static bool IsValid(this IGnssBroadcastBinaryMessage message)
        {
            return message.IsLatitudeAndLongitudeValidRange() && message.UserId.IsValidMMSI()
                && message.DifferentialCorrectionData.Length > 0 && message.MessageId == AisMessageType.GnssBroadcastBinaryMessage;
        }
    }
}
