using Njord.Ais.Enums;
using Njord.Ais.Messages;
using Njord.Server.Grains.Abstracts;
using Njord.Server.Grains.Interfaces;
using Njord.Server.Grains.States;
using Orleans.Runtime;

namespace Njord.Server.Grains
{
    public class PilotStation : AbstractBaseStationGrain, IPilotStation
    {
        private readonly IPersistentState<PilotStationState> _state;

        public PilotStation([PersistentState(nameof(PilotStation))] IPersistentState<PilotStationState> state, ILogger<PilotStation> logger) : base(logger)
        {
            _state = state;

            Map(_ => ProcessBaseStationReport((IBaseStationReportMessage)_, _state),
               AisMessageType.BaseStationReport,
               AisMessageType.CoordinatedUtcBaseStationReport);
            Map(_ => ProcessGnssBinaryMessage((IGnssBroadcastBinaryMessage)_, _state), AisMessageType.GnssBroadcastBinaryMessage);

            Map(_ => ProcessLongRange((ILongRangeAisBroadcastMessage)_, _state), AisMessageType.LongRangeAisBroadcastMessage);
        }
    }
}
