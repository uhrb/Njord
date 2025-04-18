using Njord.Server.Grains.Abstracts;
using Njord.Server.Grains.Interfaces;

namespace Njord.Server.Grains
{
    public class AutonomousDeviceGroupB : AbstractMaritimeGrain, IAutonomousDeviceGroupB
    {
        public AutonomousDeviceGroupB(ILogger<AutonomousDeviceGroupB> logger) : base(logger)
        {
        }
    }
}
