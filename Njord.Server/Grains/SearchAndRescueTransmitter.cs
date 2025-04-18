using Njord.Ais.Enums;
using Njord.Ais.Extensions.Messages;
using Njord.Ais.Messages;
using Njord.Server.Grains.Abstracts;
using Njord.Server.Grains.Interfaces;
using Njord.Server.Grains.States;
using Orleans.Runtime;

namespace Njord.Server.Grains
{
    public class SearchAndRescueTransmitter : AbstractMaritimeGrain, ISearchAndRescueTransmitter
    {
        private readonly IPersistentState<SearchAndRescueTransmitterState> _state;

        public SearchAndRescueTransmitter([PersistentState(nameof(SearchAndRescueTransmitter))] IPersistentState<SearchAndRescueTransmitterState> state,
            ILogger<SearchAndRescueTransmitter> logger) : base(logger)
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
