using Njord.Ais.Enums;
using Njord.Ais.Extensions.Types;
using Njord.Ais.Messages;

namespace Njord.Ais.Extensions.Messages
{
    public static class CoordinatedUniversalTimeDateInquiryMessageExtensions
    {
        public static bool IsValid(this ICoordinatedUniversalTimeDateInquiryMessage message)
        {
            return message.UserId.IsValidMMSI()
                && message.DestinationId.IsValidMMSI()
                && message.MessageId == AisMessageType.CoordinatedUTCInquiry;
        }
    }
}
