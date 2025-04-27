using Njord.Ais.Enums;
using Njord.Ais.Messages;
using Njord.Server.Grains.Abstracts;
using Njord.Server.Grains.Interfaces;
using Njord.Server.Grains.States;

namespace Njord.Server.Grains
{
    public class BaseStation : AbstractBaseStationGrain, IBaseStation
    {
        private readonly IPersistentState<BaseStationState> _state;

        public BaseStation([PersistentState(nameof(BaseStation))] IPersistentState<BaseStationState> state, ILogger<BaseStation> logger) : base(logger)
        {
            _state = state;

            Map(_ => ProcessBaseStationReport((IBaseStationReportMessage)_, _state), 
                AisMessageType.BaseStationReport,
                AisMessageType.CoordinatedUtcBaseStationReport);
            Map(_ => ProcessGnssBinaryMessage((IGnssBroadcastBinaryMessage)_, _state), AisMessageType.GnssBroadcastBinaryMessage);

            Map(_ => ProcessLongRange((ILongRangeAisBroadcastMessage)_, _state), AisMessageType.LongRangeAisBroadcastMessage);
            Map(_ => ProcessStaticDataReport((IStaticDataReportMessage) _, _state), AisMessageType.StaticDataReport);
        }
    }
}
