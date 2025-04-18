using Njord.Ais.Enums;
using Njord.Ais.Extensions.Interfaces;
using Njord.Ais.Extensions.Types;
using Njord.Ais.Messages;

namespace Njord.Ais.Extensions.Messages
{
    public static class AddressedBinaryMessageExtensions
    {
        public static bool IsValid(this IAddressedBinaryMessage message)
        {
            return message.UserId.IsValidMMSI()
                && message.DestinationId.IsValidMMSI()
                && message.MessageId == AisMessageType.AddressedBinaryMessage
                && message.ApplicationIdentifier.IsValid()
                && message.SequenceNumber <=3
                && message.ApplicationData.Length > 0;

        }
    }
}
