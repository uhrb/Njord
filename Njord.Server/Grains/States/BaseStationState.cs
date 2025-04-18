using Njord.Ais.Enums;
using Njord.Server.Grains.States.Abstracts;

namespace Njord.Server.Grains.States
{
    [GenerateSerializer]
    [Alias("Njord.Server.Grains.States.BaseStationState")]
    public record BaseStationState : AbstractPositionState
    {
    }
}
