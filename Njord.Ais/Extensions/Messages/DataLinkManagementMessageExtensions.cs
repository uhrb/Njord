using Njord.Ais.Enums;
using Njord.Ais.Extensions.Interfaces;
using Njord.Ais.Extensions.Types;
using Njord.Ais.Messages;

namespace Njord.Ais.Extensions.Messages
{
    public static class DataLinkManagementMessageExtensions
    {
        public static bool IsValid(this IDataLinkManagementMessage message)
        {
            var val = message.UserId.IsValidMMSI() 
                && message.MessageId == AisMessageType.DataLinkManagementMessage
                && message.SlotOffsets.Any();

            foreach (var slot in message.SlotOffsets)
            {
                val = val && slot.IsValid();
            }

            return val;
        }

        public static bool IsInterrogatedButNoDataLinkInformationAvailiable(this IDataLinkManagementMessage message)
        {
            return message.SlotOffsets.First().IsInterrogatedButNoDatalinkInformationAvailiable();
        }
    }
}
