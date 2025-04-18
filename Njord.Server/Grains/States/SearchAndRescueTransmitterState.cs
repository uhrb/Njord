using Njord.Server.Grains.States.Abstracts;

namespace Njord.Server.Grains.States
{
    [GenerateSerializer]
    [Alias("Njord.Server.Grains.States.SearchAndRescueTransmitterState")]
    public record SearchAndRescueTransmitterState : AbstractMovingPositionState
    {
    }
}
