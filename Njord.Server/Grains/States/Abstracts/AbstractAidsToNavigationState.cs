using Njord.Ais.Enums;
using Njord.Ais.Interfaces;

namespace Njord.Server.Grains.States.Abstracts
{
    [GenerateSerializer]
    [Alias("Njord.Server.Grains.States.Abstracts.AbstractAidsToNavigationState")]
    public abstract record AbstractAidsToNavigationState : AbstractPositionState
    {
        [Id(0)]
        public string? Name { get; set; }
        [Id(1)]
        public string? NameExtension { get; set; }
        [Id(2)]
        public IDimensions? Dimensions { get; set; }
        [Id(3)]
        public bool IsVirtualDevice { get; set; }
        [Id(4)]
        public AidsToNavigationType TypeOfAidsToNavigation { get; set; } = AidsToNavigationType.NotSpecified;
        [Id(5)]
        public IAidsToNavigationState? AtoNStatus { get; set; }
        [Id(6)]
        public bool IsOffPosition { get; set; }
    }
}
