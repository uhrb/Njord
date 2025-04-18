using Njord.Server.Grains.Abstracts;
using Njord.Server.Grains.Interfaces;

namespace Njord.Server.Grains
{
    public class ManOverBoard : AbstractMaritimeGrain, IManOverBoard
    {
        public ManOverBoard(ILogger<ManOverBoard> logger) : base(logger)
        {
        }
    }
}
