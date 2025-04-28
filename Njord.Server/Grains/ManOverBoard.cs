using Njord.Ais.Enums;
using Njord.Ais.Messages;
using Njord.Server.Grains.Abstracts;
using Njord.Server.Grains.Interfaces;
using Njord.Server.Grains.States;
using Njord.Ais.Extensions.Messages;

namespace Njord.Server.Grains
{
    public class ManOverBoard : AbstractMaritimeGrain, IManOverBoard
    {
        private readonly IPersistentState<ManOverBoardState> _state;

        public ManOverBoard([PersistentState(nameof(ManOverBoard))] IPersistentState<ManOverBoardState> state,
            ILogger<ManOverBoard> logger) : base(logger)
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
