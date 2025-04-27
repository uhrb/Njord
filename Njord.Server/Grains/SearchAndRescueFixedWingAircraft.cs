using Njord.Ais.Enums;
using Njord.Ais.Messages;
using Njord.Server.Grains.Abstracts;
using Njord.Server.Grains.Interfaces;
using Njord.Server.Grains.States;

namespace Njord.Server.Grains
{
    public class SearchAndRescueFixedWingAircraft : AbstractSearchAndRescueGrain, ISearchAndRescueFixedWingAircraft
    {
        private readonly IPersistentState<FixedWindSARAircraftState> _state;

        public SearchAndRescueFixedWingAircraft([PersistentState(nameof(SearchAndRescueFixedWingAircraft))] IPersistentState<FixedWindSARAircraftState> state,
            ILogger<SearchAndRescueFixedWingAircraft> logger) : base(logger)
        {
            _state = state;
            Map(_ => ProcessStandardSearchAndRescueReport((IStandardSARAircraftPositionReportMessage)_, _state), AisMessageType.StandardSearchAndRescueAircraftReport);
            Map(_ => ProcessStaticDataReport((IStaticDataReportMessage)_, _state), AisMessageType.StaticDataReport);
        }
    }
}
