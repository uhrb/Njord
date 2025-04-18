using Njord.Ais.Extensions.Interfaces;
using Njord.Ais.Extensions.Types;
using Njord.Ais.Messages;

namespace Njord.Ais.Extensions.Messages
{
    public static class GroupAssignmentCommandMessageExtensions
    {
        public static bool IsValid(this IGroupAssignmentCommandMessage message)
        {
            return message.UserId.IsValidMMSI()
                && message.GeoArea.IsValid()
                && message.QuietTime <= 15; 
        }
    }
}
