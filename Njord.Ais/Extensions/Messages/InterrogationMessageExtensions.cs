using Njord.Ais.Enums;
using Njord.Ais.Extensions.Interfaces;
using Njord.Ais.Extensions.Types;
using Njord.Ais.Messages;

namespace Njord.Ais.Extensions.Messages
{
    public static class InterrogationMessageExtensions
    {
        public static bool IsValid(this IInterrogationMessage message)
        {
            var val = message.UserId.IsValidMMSI()
                && message.MessageId == AisMessageType.Interrogation
                && message.Interrogations.Any();

            foreach (var item in message.Interrogations)
            {
                val = val && item.IsValid();
            }
            return val;
        }
    }
}
