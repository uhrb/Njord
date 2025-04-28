using Njord.Ais.Enums;
using Njord.Ais.Messages;
using Njord.Server.Grains.Abstracts;
using Njord.Server.Grains.Interfaces;
using Njord.Server.Grains.States;
using Njord.Ais.Extensions.Messages;

namespace Njord.Server.Grains
{
    public class HandheldVHF : AbstractMaritimeGrain, IHandheldVHF
    {
        private readonly IPersistentState<HandheldVHFState> _state;

        public HandheldVHF([PersistentState(nameof(HandheldVHF))] IPersistentState<HandheldVHFState> state,
            ILogger<HandheldVHF> logger) : base(logger)
        {
            _state = state;

            Map(_ => ProcessPositionReport((IPositionReportMessage)_),
                AisMessageType.PositionReportAssignedScheduled,
                AisMessageType.PositionReportScheduled,
                AisMessageType.PositionReportSpecial);
        }

        private async Task ProcessPositionReport(IPositionReportMessage _)
        {
            if (false == _.IsValid()) return;
            UpdateFromMovingPositionMessage((IPositionReportMessage)_, _state);
            await _state.WriteStateAsync();
        }
    }
}
