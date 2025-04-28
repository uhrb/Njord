using Njord.Ais.Enums;
using Njord.Ais.Messages;
using Njord.Ais.Extensions.Messages;
using Njord.Server.Grains.Abstracts;
using Njord.Server.Grains.Interfaces;
using Njord.Server.Grains.States;

namespace Njord.Server.Grains
{
    public class EmergencyPositionIdentificationSystem : AbstractMaritimeGrain, IEmergencyPositionIdentificationSystem
    {
        private readonly IPersistentState<EmergencyPositionIdentificationSystemState> _state;

        public EmergencyPositionIdentificationSystem([PersistentState(nameof(EmergencyPositionIdentificationSystem))] IPersistentState<EmergencyPositionIdentificationSystemState> state,
            ILogger<EmergencyPositionIdentificationSystem> logger) : base(logger)
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
