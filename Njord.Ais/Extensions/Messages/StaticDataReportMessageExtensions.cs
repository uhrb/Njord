using Njord.Ais.Enums;
using Njord.Ais.Extensions.Types;
using Njord.Ais.Messages;

namespace Njord.Ais.Extensions.Messages
{
    public static class StaticDataReportMessageExtensions
    {
        public static bool IsValid(this IStaticDataReportMessage message)
        {
            return message.UserId.IsValidMMSI()
                && message.MessageId == AisMessageType.StaticDataReport;
        }
    }
}
