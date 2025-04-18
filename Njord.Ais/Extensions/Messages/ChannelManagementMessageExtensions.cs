using Njord.Ais.Extensions.Interfaces;
using Njord.Ais.Extensions.Types;
using Njord.Ais.Messages;

namespace Njord.Ais.Extensions.Messages
{
    public static class ChannelManagementMessageExtensions
    {
        public static bool IsValid(this IChannelManagementMessage message)
        {
            var val = message.MessageId == Enums.AisMessageType.ChannelManagement
                && message.UserId.IsValidMMSI()
                && message.TransitionalZoneSize >= 4;

            if (message.IsAddressed)
            {
                val = val && (message.AddressedStations != null);
                if (message.AddressedStations != null)
                {
                    foreach (var station in message.AddressedStations)
                    {
                        val = val && station.IsValidMMSI();
                    }
                }
            }
            else
            {
                val = val && (message.Area != null);
                if (message.Area != null)
                {
                    val = val && message.Area.IsValid();
                }
            }

            return val;
        }
    }
}
