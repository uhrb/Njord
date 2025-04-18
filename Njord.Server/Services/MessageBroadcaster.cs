using Microsoft.AspNetCore.SignalR;
using Njord.Ais.Extensions.Interfaces;
using Njord.Server.Grains.States.Abstracts;
using Njord.Server.Hubs;

namespace Njord.Server.Services
{
    public class MessageBroadcaster : IMessageBroadcaster
    {
        private readonly ILogger<MessageBroadcaster> _logger;
        private readonly IHubContext<MapUpdatesHub> _hub;

        public MessageBroadcaster(ILogger<MessageBroadcaster> logger, IHubContext<MapUpdatesHub> hub)
        {
            _logger = logger;
            _hub = hub;
        }

        public async Task BroadcastUpdateAsync(string entityType, string entityId, AbstractPositionState state, object? exclusive = default)
        {
            if (state.Longitude != LongitudeAndLatitudeExtensions.LongitudeNotAvailable
                     && state.Latitude != LongitudeAndLatitudeExtensions.LatitudeNotAvailable)
            {
                await _hub.Clients.All.SendAsync("Update", entityType, entityId, state);
            }
        }
    }
}
