using Njord.Server.Grains.States.Abstracts;
using Njord.Server.Services;
using System.Collections.Concurrent;

namespace Njord.Server.Intstrumentation
{
    public sealed class InMemoryGrainStorage : IGrainStorageWithQueries, ILifecycleParticipant<ISiloLifecycle>
    {
        private readonly IMessageBroadcaster _broadcaster;
        private readonly ConcurrentDictionary<string, AbstractPositionState> _states;


        public InMemoryGrainStorage(IMessageBroadcaster broadcaster)
        {
            _broadcaster = broadcaster;
            _states = [];
        }

        public Task ClearStateAsync<T>(string stateName, GrainId grainId, IGrainState<T> grainState)
        {
            _states.TryRemove(GetKeyString(stateName, grainId), out _);
            return Task.CompletedTask;
        }

        public void Participate(ISiloLifecycle lifecycle)
        {
            // nothing;
        }

        public Task ReadStateAsync<T>(string stateName, GrainId grainId, IGrainState<T> grainState)
        {
            _states.TryGetValue(GetKeyString(stateName, grainId), out var state);
            if (state != null)
            {
                grainState.RecordExists = true;
                grainState.State = (T)(object)state;

            }
            else
            {
                grainState.RecordExists = false;
                grainState.State = Activator.CreateInstance<T>()!;
            }
            return Task.CompletedTask;
        }

        public async Task WriteStateAsync<T>(string stateName, GrainId grainId, IGrainState<T> grainState)
        {
            if (grainState.State == null)
            {
                return;
            }
            var typedState = (AbstractPositionState)(object)grainState.State;
            typedState.Updated = DateTime.UtcNow;
            _states.AddOrUpdate(GetKeyString(stateName, grainId), typedState, (k,v) => typedState);
            await _broadcaster.BroadcastUpdateAsync(stateName, $"{grainId.Key}", (AbstractPositionState)(object)grainState.State);
        }

        private static string GetKeyString(string grainType, GrainId grainId)
        {
            return $"{grainType}.{grainId.Key}";
        }

        public IEnumerable<GrainStateStored> GetAllStatesByGrainType(string grainType)
        {
            var pairs = _states.Where(_ => _.Key.StartsWith(grainType));
            foreach(var entry in pairs)
            {
                var splitted = entry.Key.Split('.');    
                yield return new GrainStateStored
                {
                    GrainType = splitted[0],
                    GrainId = splitted[1],
                    GrainState = entry.Value
                };
            }
        }
    }
}
