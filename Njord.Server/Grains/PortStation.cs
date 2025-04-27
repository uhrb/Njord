using Njord.Ais.Enums;
using Njord.Ais.Messages;
using Njord.Server.Grains.Abstracts;
using Njord.Server.Grains.Interfaces;
using Njord.Server.Grains.States;
using Orleans.Runtime;

namespace Njord.Server.Grains
{
    public class PortStation : AbstractBaseStationGrain, IPortStation
    {
        private readonly IPersistentState<PortStationState> _state;

        public PortStation([PersistentState(nameof(PortStation))] IPersistentState<PortStationState> state, ILogger<PortStation> logger) : base(logger)
        {
            _state = state;

            Map(_ => ProcessBaseStationReport((IBaseStationReportMessage)_, _state),
               AisMessageType.BaseStationReport,
               AisMessageType.CoordinatedUtcBaseStationReport);
            Map(_ => ProcessGnssBinaryMessage((IGnssBroadcastBinaryMessage)_, _state), AisMessageType.GnssBroadcastBinaryMessage);

            Map(_ => ProcessLongRange((ILongRangeAisBroadcastMessage)_, _state), AisMessageType.LongRangeAisBroadcastMessage);
            Map(_ => ProcessStaticDataReport((IStaticDataReportMessage)_, _state), AisMessageType.StaticDataReport);
        }
    }
}
