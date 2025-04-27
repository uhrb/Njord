using Njord.Server.Grains.States.Abstracts;

namespace Njord.Server.Grains.States
{
    [GenerateSerializer]
    [Alias("Njord.Server.Grains.States.RepeaterStationState")]
    public record RepeaterStationState : AbstractStationState
    {
    }
}
