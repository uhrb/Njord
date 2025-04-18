using Njord.Ais.Enums;
using Njord.Ais.Messages;
using Njord.Server.Grains.Abstracts;
using Njord.Server.Grains.Interfaces;
using Njord.Server.Grains.States;
using Orleans.Runtime;

namespace Njord.Server.Grains
{
    public class PhysicalAidsToNavigation : AbstractAidsToNavigationGrain, IPhysicalAidsToNavigation
    {
        private readonly IPersistentState<PhysicalAidsToNavigationState> _state;

        public PhysicalAidsToNavigation([PersistentState(nameof(PhysicalAidsToNavigation))] IPersistentState<PhysicalAidsToNavigationState> state, ILogger<PhysicalAidsToNavigation> logger) : base(logger)
        {
            _state = state;
            Map(_ => ProcessAidsToNavigationReport((IAidsToNavigationReportMessage)_, _state), AisMessageType.AidsToNavigationReport);
        }
    }
}
