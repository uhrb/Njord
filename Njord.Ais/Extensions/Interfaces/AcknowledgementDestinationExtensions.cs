using Njord.Ais.Extensions.Types;
using Njord.Ais.Interfaces;

namespace Njord.Ais.Extensions.Interfaces
{
    public static class AcknowledgementDestinationExtensions
    {
        public static bool IsValid(this IAcknowledgementDestination dest)
        {
            return dest.DestinationId.IsValidMMSI()
                && dest.SequenceNumber <= 3;
        }
    }
}
