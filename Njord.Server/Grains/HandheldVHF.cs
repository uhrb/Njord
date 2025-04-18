using Njord.Server.Grains.Abstracts;
using Njord.Server.Grains.Interfaces;

namespace Njord.Server.Grains
{
    public class HandheldVHF : AbstractMaritimeGrain, IHandheldVHF
    {
        public HandheldVHF(ILogger<HandheldVHF> logger) : base(logger)
        {
        }
    }
}
