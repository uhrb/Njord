using Njord.Ais.Enums;
using Njord.Ais.Extensions.Interfaces;
using Njord.Ais.Extensions.Types;
using Njord.Ais.Messages;

namespace Njord.Ais.Extensions.Messages
{
    public static class BinaryAcknowledgeMessageExtensions 
    {
        public static bool IsValid(this IBinaryAcknowledgeMessage message)
        {
            var val = message.UserId.IsValidMMSI()
                && message.Acknowledgements.Any()
                && (message.MessageId == AisMessageType.BinaryAcknowledge || message.MessageId == AisMessageType.BinaryAcknowledgeSafety);
            foreach(var item in message.Acknowledgements)
            {
                val = val && item.IsValid();
            }

            return val;
        }
    }
}
