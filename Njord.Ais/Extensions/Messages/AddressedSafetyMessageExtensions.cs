using Njord.Ais.Enums;
using Njord.Ais.Extensions.Types;
using Njord.Ais.Messages;

namespace Njord.Ais.Extensions.Messages
{
    public static class AddressedSafetyMessageExtensions
    {
        public static bool IsValid(this IAddressedSafetyMessage message)
        {
            return message.UserId.IsValidMMSI()
                && message.DestinationId.IsValidMMSI()
                && message.MessageId == AisMessageType.AddressedSafetyMessage
                && message.SequenceNumber <= 3;
        }
    }
}
