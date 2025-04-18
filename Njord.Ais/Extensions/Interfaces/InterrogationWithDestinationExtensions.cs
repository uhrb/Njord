using Njord.Ais.Extensions.Types;
using Njord.Ais.Interfaces;

namespace Njord.Ais.Extensions.Interfaces
{
    public static class InterrogationWithDestinationExtensions
    {
        public static bool IsValid(this IInterrogationWithDestination destination)
        {
            return destination.DestinationId.IsValidMMSI()
                && (byte)destination.MessageType > 0
                && Enum.IsDefined(destination.MessageType);
        }
    }
}
