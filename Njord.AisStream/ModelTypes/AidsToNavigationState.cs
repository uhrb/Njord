using Njord.Ais.Enums;
using Njord.Ais.Interfaces;
using Njord.AisStream.Converters;
using System.Text.Json.Serialization;

namespace Njord.AisStream.ModelTypes
{

    [JsonConverter(typeof(JsonAidsToNavigationStateConverter))]
    public record AidsToNavigationState : IAidsToNavigationState
    {
        public required bool IsAlarmState { get; init; }
        public required AidsToNavigationLightsStatus LightsState { get; init; }
        public required AidsToNavigationRACONStatus RACONState { get; init; }
    }
}
