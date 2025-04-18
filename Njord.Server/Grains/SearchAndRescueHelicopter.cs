using Njord.Ais.Enums;
using Njord.Ais.Messages;
using Njord.Server.Grains.Abstracts;
using Njord.Server.Grains.Interfaces;
using Njord.Server.Grains.States;
using Orleans.Runtime;

namespace Njord.Server.Grains
{
    public class SearchAndRescueHelicopter : AbstractSearchAndRescueGrain, ISearchAndRescueHelicopter
    {
        private readonly IPersistentState<HelicopterSARAircraftState> _state;

        public SearchAndRescueHelicopter([PersistentState(nameof(SearchAndRescueHelicopter))] IPersistentState<HelicopterSARAircraftState> state,
            ILogger<SearchAndRescueHelicopter> logger) : base(logger)
        {
            _state = state;
            Map(_ => ProcessStandardSearchAndRescueReport((IStandardSARAircraftPositionReportMessage)_, _state), AisMessageType.StandardSearchAndRescueAircraftReport);
        }
    }
}
