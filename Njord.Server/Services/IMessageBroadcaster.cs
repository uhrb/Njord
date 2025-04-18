using Njord.Server.Grains.States.Abstracts;

namespace Njord.Server.Services
{
    public interface IMessageBroadcaster
    {
        public Task BroadcastUpdateAsync(string entityType, string entityId, AbstractPositionState state, object? exclusive = default);
    }
}