using Njord.Ais.Extensions.Messages;
using Njord.Ais.Messages;
using Njord.Server.Grains.States.Abstracts;

namespace Njord.Server.Grains.Abstracts
{
    public abstract class AbstractAidsToNavigationGrain : AbstractMaritimeGrain
    {
        protected AbstractAidsToNavigationGrain(ILogger logger) : base(logger)
        {
        }

        protected async Task ProcessAidsToNavigationReport<T>(IAidsToNavigationReportMessage _, IPersistentState<T> state) where T : AbstractAidsToNavigationState
        {
            if (false == _.IsValid()) return;

            state.State.Longitude = _.Longitude;
            state.State.Latitude = _.Latitude;
            state.State.IsPositionAccuracyHigh = _.IsPositionAccuracyHigh;
            state.State.IsRaimInUse = _.IsRaimInUse;
            state.State.FixingDeviceType = _.FixingDeviceType;
            state.State.Name = _.Name;
            state.State.NameExtension = _.NameExtension;
            state.State.Dimensions = _.Dimensions;
            state.State.IsVirtualDevice = _.IsVirtualDevice;
            state.State.TypeOfAidsToNavigation = _.TypeOfAidsToNavigation;
            state.State.AtoNStatus = _.AtoNStatus;
            state.State.IsOffPosition = _.IsOffPosition;

            await state.WriteStateAsync();
        }
    }
}
