using Njord.Ais.Enums;
using Njord.Ais.Extensions.Interfaces;
using Njord.Ais.Extensions.Types;
using Njord.Ais.Messages;

namespace Njord.Ais.Extensions.Messages
{
    public static class AssignedModeCommandMessageExtensions
    {
        public static bool IsValid(this IAssignedModeCommandMessage message)
        {
            var val = message.MessageId == AisMessageType.AssignedModeCommand
                && message.UserId.IsValidMMSI()
                && message.AssignedDestinations.Any();

            foreach (var item in message.AssignedDestinations)
            {
                val = val && item.IsValid();
            }

            return val;
        }
    }
}
