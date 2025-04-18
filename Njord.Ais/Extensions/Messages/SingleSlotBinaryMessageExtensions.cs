using Njord.Ais.Enums;
using Njord.Ais.Extensions.Interfaces;
using Njord.Ais.Extensions.Types;
using Njord.Ais.Messages;

namespace Njord.Ais.Extensions.Messages
{
    public static class SingleSlotBinaryMessageExtensions
    {
        public static bool IsValid(this ISingleSlotBinaryMessage message)
        {
            var val = message.UserId.IsValidMMSI()
               && message.MessageId == AisMessageType.SingleSlotBinaryMessage
               && message.Data.IsEmpty == false;

            if (message.IsAddressed)
            {
                val = val && message.DestinationId.IsValidMMSI();
            }

            if (message.IsApplicationIdentifierEncodedData)
            {
                val = val && message.ApplicationIdentifier != null && message.ApplicationIdentifier.IsValid();
            }

            return val;
        }
    }
}
