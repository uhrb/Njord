using Njord.Ais.Enums;
using Njord.Ais.Extensions.Types;
using Njord.Server.Grains.Interfaces;

namespace Njord.Server.Grains.Instrumentation
{
    public class GrainAccessor : IGrainAccessor
    {
        private readonly IGrainFactory _factory;

        public GrainAccessor(IGrainFactory factory)
        {
            _factory = factory;
        }

        public IUnknownMaritimeEntity? GetGrainFromMMSI(string key)
        {
            var fromMMSI = key.ToMaritimeEntityType();
            IUnknownMaritimeEntity? concreteDevice = fromMMSI switch
            {
                MaritimeEntityType.CoastStation => _factory.GetGrain<ICoastStation>(key),
                MaritimeEntityType.PortStation => _factory.GetGrain<IPortStation>(key),
                MaritimeEntityType.PilotStation => _factory.GetGrain<IPilotStation>(key),
                MaritimeEntityType.RepeaterStation => _factory.GetGrain<IRepeaterStation>(key),
                MaritimeEntityType.BaseStation => _factory.GetGrain<IBaseStation>(key),
                MaritimeEntityType.ShipStationGroup => _factory.GetGrain<IShipStationGroup>(key),
                MaritimeEntityType.SearchAndRescueFixedWingAircraft => _factory.GetGrain<ISearchAndRescueFixedWingAircraft>(key),
                MaritimeEntityType.SearchAndRescueHelicopter => _factory.GetGrain<ISearchAndRescueHelicopter>(key),
                MaritimeEntityType.Vessel => _factory.GetGrain<IVessel>(key),
                MaritimeEntityType.HandheldVHF => _factory.GetGrain<IHandheldVHF>(key),
                MaritimeEntityType.PhysicalAidsToNavigation => _factory.GetGrain<IPhysicalAidsToNavigation>(key),
                MaritimeEntityType.VirtualAidsToNavigation => _factory.GetGrain<IVirtualAidsToNavigation>(key),
                MaritimeEntityType.MobileAidsToNavigation => _factory.GetGrain<IMobileAidsToNavigation>(key),
                MaritimeEntityType.SearchAndRescueTransmitter => _factory.GetGrain<ISearchAndRescueTransmitter>(key),
                MaritimeEntityType.ManOverBoard => _factory.GetGrain<IManOverBoard>(key),
                MaritimeEntityType.EmergencyPositionIdentificationSystem => _factory.GetGrain<IEmergencyPositionIdentificationSystem>(key),
                MaritimeEntityType.AutonomousDeviceGroupB => _factory.GetGrain<IAutonomousDeviceGroupB>(key),
                _ => null,
            };

            return concreteDevice;
        }
    }
}
