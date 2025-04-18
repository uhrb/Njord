using Njord.Ais.Enums;
using Njord.Ais.Extensions.Types;
using Njord.Ais.Messages;

namespace Njord.Ais.Extensions.Messages
{
    public static class SafetyRelatedBroadcastMessageExtensions
    {
        public static bool IsValid(this ISafetyRelatedBroadcastMessage message)
        {
            return message.MessageId == AisMessageType.SafetyBroadcastMessage
                && message.UserId.IsValidMMSI();
        }
    }
}
