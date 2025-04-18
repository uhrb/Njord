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
    }
}
