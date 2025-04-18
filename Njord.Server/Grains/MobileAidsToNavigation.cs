using Njord.Ais.Enums;
using Njord.Ais.Messages;
using Njord.Server.Grains.Abstracts;
using Njord.Server.Grains.Interfaces;
using Njord.Server.Grains.States;
using Orleans.Runtime;

namespace Njord.Server.Grains
{
    public class MobileAidsToNavigation : AbstractAidsToNavigationGrain, IMobileAidsToNavigation
    {
        private readonly IPersistentState<MobileAidsToNavigationState> _state;

        public MobileAidsToNavigation([PersistentState(nameof(MobileAidsToNavigation))] IPersistentState<MobileAidsToNavigationState> state, ILogger<MobileAidsToNavigation> logger) : base(logger)
        {
            _state = state;
            Map(_ => ProcessAidsToNavigationReport((IAidsToNavigationReportMessage)_, _state), AisMessageType.AidsToNavigationReport);
        }
    }
}
