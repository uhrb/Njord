using Njord.Ais.Extensions.Messages;
using Njord.Ais.Messages;
using Njord.Server.Grains.States.Abstracts;
using Orleans.Runtime;

namespace Njord.Server.Grains.Abstracts
{
    public abstract class AbstractSearchAndRescueGrain : AbstractMaritimeGrain
    {
        public AbstractSearchAndRescueGrain(ILogger logger) : base(logger)
        {
        }

        protected async Task ProcessStandardSearchAndRescueReport<T>(IStandardSARAircraftPositionReportMessage _, IPersistentState<T> state) where T : AbstractSearchAndRescueState
        {
            if (false == _.IsValid()) return;

            state.State.Altitude = _.Altitude;
            state.State.CourseOverGround = _.CourseOverGround;
            state.State.SpeedOverGround = _.SpeedOverGround;
            state.State.IsAltitudeSensorTypeBarometric = _.IsAltitudeSensorTypeBarometric;
            state.State.IsPositionAccuracyHigh = _.IsPositionAccuracyHigh;
            state.State.IsRaimInUse = _.IsRaimInUse;
            state.State.Latitude = _.Latitude;
            state.State.Longitude = _.Longitude;

            await state.WriteStateAsync();
        }
    }
}
