using Microsoft.AspNetCore.SignalR;
using Njord.Ais.Extensions.Interfaces;
using Njord.Server.Intstrumentation;

namespace Njord.Server.Hubs
{
    public class MapUpdatesHub : Hub
    {
        private readonly ILogger<MapUpdatesHub> _logger;
        private readonly IGrainStorageWithQueries _queries;

        public MapUpdatesHub(ILogger<MapUpdatesHub> logger, IGrainStorageWithQueries queries)
        {
            _logger = logger;
            _queries = queries;
        }

        public override Task OnConnectedAsync()
        {
            _logger.LogInformation("Client connected {ConnectionId}", Context.ConnectionId);
            return base.OnConnectedAsync();
        }


        public async Task CommandSendAllStatesByType(string type)
        {
            var states = _queries.GetAllStatesByGrainType(type);
            foreach(var state in states)
            {
                if (state.GrainState.Longitude != LongitudeAndLatitudeExtensions.LongitudeNotAvailable
                     && state.GrainState.Latitude != LongitudeAndLatitudeExtensions.LatitudeNotAvailable)
                {
                    await Clients.Caller.SendAsync("Update", state.GrainType, state.GrainId, state.GrainState);
                }
            }
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            _logger.LogInformation("Client disconnected {ConnectionId} {exception}", Context.ConnectionId, exception);
            return base.OnDisconnectedAsync(exception);
        }
    }
}
