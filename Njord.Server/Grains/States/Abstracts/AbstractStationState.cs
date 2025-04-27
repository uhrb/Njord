namespace Njord.Server.Grains.States.Abstracts
{
    [GenerateSerializer]
    [Alias("Njord.Server.Grains.States.Abstracts.AbstractStationState")]
    public abstract record AbstractStationState : AbstractPositionState
    {
        [Id(0)]
        public string? Name { get; set; }

        [Id(1)]
        public string? CallSign { get; set; }
    }
}
