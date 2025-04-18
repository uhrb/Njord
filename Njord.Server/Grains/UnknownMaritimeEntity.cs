using Njord.Ais.Enums;
using Njord.Ais.Extensions.Types;
using Njord.Ais.Interfaces;
using Njord.Server.Grains.Abstracts;
using Njord.Server.Grains.Instrumentation;
using Njord.Server.Grains.Interfaces;

namespace Njord.Server.Grains
{
    public class UnknownMaritimeEntity : AbstractMaritimeGrain, IUnknownMaritimeEntity
    {
        private readonly ILogger<UnknownMaritimeEntity> _logger;

        public UnknownMaritimeEntity(ILogger<UnknownMaritimeEntity> logger) : base(logger)
        {
            _logger = logger;
        }

        public override async Task ProcessMessage(IMessageId message)
        {
            var key = this.GetPrimaryKeyString();
            var fromMMSI = key.ToMaritimeEntityType();
            IUnknownMaritimeEntity? concreteDevice = fromMMSI switch
            {
                MaritimeEntityType.CoastStation => GrainFactory.GetGrain<ICoastStation>(key),
                MaritimeEntityType.PortStation => GrainFactory.GetGrain<IPortStation>(key),
                MaritimeEntityType.PilotStation => GrainFactory.GetGrain<IPilotStation>(key),
                MaritimeEntityType.RepeaterStation => GrainFactory.GetGrain<IRepeaterStation>(key),
                MaritimeEntityType.BaseStation => GrainFactory.GetGrain<IBaseStation>(key),
                MaritimeEntityType.ShipStationGroup => GrainFactory.GetGrain<IShipStationGroup>(key),
                MaritimeEntityType.SearchAndRescueFixedWingAircraft => GrainFactory.GetGrain<ISearchAndRescueFixedWingAircraft>(key),
                MaritimeEntityType.SearchAndRescueHelicopter => GrainFactory.GetGrain<ISearchAndRescueHelicopter>(key),
                MaritimeEntityType.Vessel => GrainFactory.GetGrain<IVessel>(key),
                MaritimeEntityType.HandheldVHF => GrainFactory.GetGrain<IHandheldVHF>(key),
                MaritimeEntityType.PhysicalAidsToNavigation => GrainFactory.GetGrain<IPhysicalAidsToNavigation>(key),
                MaritimeEntityType.VirtualAidsToNavigation => GrainFactory.GetGrain<IVirtualAidsToNavigation>(key),
                MaritimeEntityType.MobileAidsToNavigation => GrainFactory.GetGrain<IMobileAidsToNavigation>(key),
                MaritimeEntityType.SearchAndRescueTransmitter => GrainFactory.GetGrain<ISearchAndRescueTransmitter>(key),
                MaritimeEntityType.ManOverBoard => GrainFactory.GetGrain<IManOverBoard>(key),
                MaritimeEntityType.EmergencyPositionIdentificationSystem => GrainFactory.GetGrain<IEmergencyPositionIdentificationSystem>(key),
                MaritimeEntityType.AutonomousDeviceGroupB => GrainFactory.GetGrain<IAutonomousDeviceGroupB>(key),
                _ => null,
            };

            if (concreteDevice != null)
            {
                await concreteDevice.ProcessMessage(message);
                _logger.LogDebug("Forwarding message to {GrainType} {Key}", concreteDevice.GetType().Name, key);
            }
            else
            {
                // _logger.LogWarning("No grain type found for {fromMMSI} {Key} {MessageType}", fromMMSI, key, message.GetType().Name);
                var grain = GrainFactory.GetGrain<IOrphanedMessageSink>(OrphanedMessageSink.OrphanedMessageSinkGrainKey);
                await grain.ProcessOrhpanedMessage(key, message);
            }

            DeactivateOnIdle();
        }
    }
}

