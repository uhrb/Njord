using Njord.Ais.Enums;
using Njord.Ais.Messages;
using Njord.Server.Grains.Abstracts;
using Njord.Server.Grains.Interfaces;
using Njord.Server.Grains.States;
using Orleans.Runtime;

namespace Njord.Server.Grains
{
    public class RepeaterStation : AbstractBaseStationGrain, IRepeaterStation
    {
        private readonly IPersistentState<RepeaterStationState> _state;

        public RepeaterStation([PersistentState(nameof(RepeaterStation))] IPersistentState<RepeaterStationState> state, ILogger<RepeaterStation> logger) : base(logger)
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
