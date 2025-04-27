using Njord.Ais.Interfaces;

namespace Njord.Server.Grains.States.Abstracts
{
    [GenerateSerializer]
    [Alias("Njord.Server.Grains.States.Abstracts.AbstractSearchAndRescueState")]
    public abstract record AbstractSearchAndRescueState : AbstractMovingPositionState
    {
        [Id(0)]
        public ushort Altitude { get; internal set; }
        [Id(1)]
        public bool IsAltitudeSensorTypeBarometric { get; internal set; }
        [Id(2)]
        public string? Name { get; internal set; }
        [Id(3)]
        public string? CallSign { get; internal set; }
    }
}
