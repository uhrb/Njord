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
        private readonly Dictionary<string, Type> _enums;

        public MapUpdatesHub(ILogger<MapUpdatesHub> logger, IGrainStorageWithQueries queries)
        {
            _logger = logger;
            _queries = queries;
            _enums = new Dictionary<string, Type> {
                { nameof(TypeOfShipAndCargoType), typeof(TypeOfShipAndCargoType) },
                { nameof(NavigationalStatus), typeof(NavigationalStatus) },
                { nameof(SpecialManoeuvreIndicator), typeof(SpecialManoeuvreIndicator) },
                { nameof(PositionFixingDeviceType), typeof(PositionFixingDeviceType) },
                { nameof(AidsToNavigationType), typeof(AidsToNavigationType) },
                { nameof(AidsToNavigationLightsStatus), typeof(AidsToNavigationLightsStatus) },
                { nameof(AidsToNavigationRACONStatus), typeof(AidsToNavigationRACONStatus) },
            };
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

        public Dictionary<object, string?> GetEnumNamesMappings(string name)
        {
            if(false == _enums.TryGetValue(name, out Type? enumType))
            {
                return [];
            }
            var values = Enum.GetValuesAsUnderlyingType(enumType);
            var mappings = new Dictionary<object, string?>();
            foreach (var enumEntryValue in values)
            {
                var enumEntryName = Enum.GetName(enumType,enumEntryValue);
                mappings.Add(enumEntryValue, SplitTitleCase(enumEntryName));
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
