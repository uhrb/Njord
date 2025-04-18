using Njord.Server.Grains.Abstracts;
using Njord.Server.Grains.Interfaces;

namespace Njord.Server.Grains
{
    public class EmergencyPositionIdentificationSystem : AbstractMaritimeGrain, IEmergencyPositionIdentificationSystem
    {
        public EmergencyPositionIdentificationSystem(ILogger<EmergencyPositionIdentificationSystem> logger) : base(logger)
        {
        }
    }
}
