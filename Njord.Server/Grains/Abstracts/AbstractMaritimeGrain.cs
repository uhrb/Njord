using Njord.Ais.Enums;
using Njord.Ais.Interfaces;
using Njord.Server.Grains.Instrumentation;
using Njord.Server.Grains.Interfaces;
using Njord.Server.Grains.States.Abstracts;
using Orleans;
using Orleans.Runtime;

namespace Njord.Server.Grains.Abstracts
{
    public abstract class AbstractMaritimeGrain : Grain, IUnknownMaritimeEntity
    {
        private readonly ILogger _logger;

        private readonly Dictionary<AisMessageType, Func<IMessageId, Task>> _map = [];

        protected AbstractMaritimeGrain(ILogger logger)
        {
            _logger = logger;
        }

        public virtual async Task ProcessMessage(IMessageId message)
        {
            var mmsi = this.GetPrimaryKeyString();
            try
            {
                if (_map.TryGetValue(message.MessageId, out Func<IMessageId, Task>? map))
                {
                    await map(message);
                }
                else
                {
                    var typeName = GetType().Name;
                    var enumName = Enum.GetName(message.MessageId);
                    _logger.LogDebug("Possibly missing mapping for Grain {typeName} for message {enumName}", typeName, enumName);
                    var sink = GrainFactory.GetGrain<IOrphanedMessageSink>(OrphanedMessageSink.OrphanedMessageSinkGrainKey);
                    await sink.ProcessOrhpanedMessage(mmsi, message);
                }

                return;

            }
            catch (Exception ex)
            {
                _logger.LogWarning("Grains failed to process message: {Message}", ex.Message);
            }

            var failGrain = GrainFactory.GetGrain<IFailedMessageSink>(FailedMessageSink.FailedMessageSinkGrainKey);
            await failGrain.ProcessFailedMessage(mmsi, message);
        }

        protected void UpdateFromPositionWithAccuracyMessage<T>(IPositionWithAccuracy message, IPersistentState<T> state) where T: AbstractPositionState
        {
            state.State.IsPositionAccuracyHigh = message.IsPositionAccuracyHigh;
            state.State.IsRaimInUse = message.IsRaimInUse;
            state.State.Latitude = message.Latitude;
            state.State.Longitude = message.Longitude;
        }

        protected void UpdateFromMovingPositionMessage<T>(IMovingPosition message, IPersistentState<T> state) where T : AbstractMovingPositionState
        {
            UpdateFromPositionWithAccuracyMessage(message, state);
            state.State.CourseOverGround = message.CourseOverGround;
            state.State.SpeedOverGround = message.SpeedOverGround;
        }

        protected void Map(Func<IMessageId, Task> func, params AisMessageType[] types)
        {
            foreach (var type in types)
            {
                _map.Add(type, func);
            }
        }
    }
}
