using Njord.Ais.Enums;
using Njord.Ais.Extensions.Interfaces;
using Njord.Ais.Extensions.Types;
using Njord.Ais.Messages;

namespace Njord.Ais.Extensions.Messages
{
    public static class LongRangeAisBroadcastMessageExtensions
    {
        public static bool IsValid(this ILongRangeAisBroadcastMessage message)
        {
            return message.IsCourseOverGroundValidRange()
                && message.IsLatitudeAndLongitudeValidRange()
                && message.UserId.IsValidMMSI()
                && message.MessageId == AisMessageType.LongRangeAisBroadcastMessage;
        }
    }
}
