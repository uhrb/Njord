using Njord.Server.Grains.Abstracts;
using Njord.Server.Grains.Interfaces;

namespace Njord.Server.Grains
{
    public class ShipStationGroup : AbstractMaritimeGrain, IShipStationGroup
    {
        public ShipStationGroup(ILogger<ShipStationGroup> logger) : base(logger)
        {
        }
    }
}
