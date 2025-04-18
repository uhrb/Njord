using Njord.Ais.Enums;
using Njord.Ais.Messages;
using Njord.Server.Grains.Abstracts;
using Njord.Server.Grains.Interfaces;
using Njord.Server.Grains.States;
using Orleans.Runtime;

namespace Njord.Server.Grains
{
    public class VirtualAidsToNavigation : AbstractAidsToNavigationGrain, IVirtualAidsToNavigation
    {
        private readonly IPersistentState<VirtualAidsToNavigationState> _state;

        public VirtualAidsToNavigation([PersistentState(nameof(VirtualAidsToNavigation))] IPersistentState<VirtualAidsToNavigationState> state, ILogger<VirtualAidsToNavigation> logger) : base(logger)
        {
            _state = state;
            Map(_ => ProcessAidsToNavigationReport((IAidsToNavigationReportMessage)_, _state), AisMessageType.AidsToNavigationReport);
        }
    }
}
