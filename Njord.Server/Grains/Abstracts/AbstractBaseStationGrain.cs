using Njord.Ais.Messages;
using Orleans.Runtime;
using Njord.Ais.Extensions.Messages;
using Njord.Server.Grains.States.Abstracts;

namespace Njord.Server.Grains.Abstracts
{
    public class AbstractBaseStationGrain : AbstractMaritimeGrain
    {
        public AbstractBaseStationGrain(ILogger logger) : base(logger)
        {
        }

        protected async Task ProcessLongRange<T>(ILongRangeAisBroadcastMessage _, IPersistentState<T> state) where T : AbstractStationState
        {
            if (false == _.IsValid()) return;

            UpdateFromPositionWithAccuracyMessage(_, state);

            await state.WriteStateAsync();
        }

        protected async Task ProcessGnssBinaryMessage<T>(IGnssBroadcastBinaryMessage _, IPersistentState<T> state) where T : AbstractStationState
        {
            if (false == _.IsValid()) return;

            state.State.Latitude = _.Latitude;
            state.State.Longitude = _.Longitude;

            await state.WriteStateAsync();
        }

        protected async Task ProcessBaseStationReport<T>(IBaseStationReportMessage _, IPersistentState<T> state) where T : AbstractStationState
        {
            if (false == _.IsValid()) return;

            UpdateFromPositionWithAccuracyMessage(_, state);
            state.State.FixingDeviceType = _.FixingDeviceType;

            await state.WriteStateAsync();
        }

        protected async Task ProcessStaticDataReport<T>(IStaticDataReportMessage _, IPersistentState<T> state) where T: AbstractStationState
        {
            if(false == _.IsValid()) return;

            if (_.IsPartA)
            {
                state.State.Name = _.Name;
            }
            else
            {
                state.State.CallSign = _.CallSign;
            }

            await state.WriteStateAsync();
        }
    }
}
