using Njord.Ais.Extensions.Interfaces;
using Njord.Ais.Extensions.Types;
using Njord.Ais.Messages;

namespace Njord.Ais.Extensions.Messages
{
    public static class AidsToNavigationReportMessageExtensions
    {
        public static bool IsValid(this IAidsToNavigationReportMessage message)
        {
            return message.UserId.IsValidMMSI()
                && message.MessageId == Enums.AisMessageType.AidsToNavigationReport
                && message.IsLatitudeAndLongitudeValidRange();
        } 
    }
}
