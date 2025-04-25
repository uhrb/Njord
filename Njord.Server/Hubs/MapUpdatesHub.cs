using Microsoft.AspNetCore.SignalR;
using Njord.Ais.Enums;
using Njord.Ais.Extensions.Interfaces;
using Njord.Server.Intstrumentation;
using System.Text.RegularExpressions;

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


        public async Task CommandSendAllStatesByTypes(string[] types)
        {
            foreach (var type in types)
            {
                var states = _queries.GetAllStatesByGrainType(type);
                foreach (var state in states)
                {
                    if (state.GrainState.Longitude != LongitudeAndLatitudeExtensions.LongitudeNotAvailable
                         && state.GrainState.Latitude != LongitudeAndLatitudeExtensions.LatitudeNotAvailable)
                    {
                        await Clients.Caller.SendAsync("Update", state.GrainType, state.GrainId, state.GrainState);
                    }
                }
            }
        }

        public Dictionary<int, string?> CommandGetShipTypeMappings()
        {
            return GetEnumNamesMappings<TypeOfShipAndCargoType>(_ => (int)_);
        }

        public Dictionary<int, string?> CommandGetNavigationStatusMappings()
        {
            return GetEnumNamesMappings<NavigationalStatus>(_ => (int)_);
        }

        public Dictionary<int, string?> CommandGetSpecialManoeuvreIndicatorMappings()
        {
            return GetEnumNamesMappings<SpecialManoeuvreIndicator>(_ => (int)_);
        }

        public Dictionary<int, string?> CommandGetPositionFixingDeviceTypeMappings()
        {
            return GetEnumNamesMappings<PositionFixingDeviceType>(_ => (int)_);
        }

        private static Dictionary<int, string?> GetEnumNamesMappings<T>(Func<T, int> conv) where T : struct, Enum
        {
            var values = Enum.GetValues<T>();
            var mappings = new Dictionary<int, string?>();
            foreach (var value in values)
            {
                var name = Enum.GetName(value);
                mappings.Add(conv(value), SplitTitleCase(name));
            }

            return mappings;
        }

        public static string? SplitTitleCase(string? input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return input;
            }

            var result = Regex.Replace(input, @"(?<=[a-z])([A-Z0-9])", " $1");
            return result;
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            _logger.LogInformation("Client disconnected {ConnectionId} {exception}", Context.ConnectionId, exception);
            return base.OnDisconnectedAsync(exception);
        }
    }
}
