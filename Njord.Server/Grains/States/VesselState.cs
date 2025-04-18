using Njord.Ais.Enums;
using Njord.Ais.Extensions.Interfaces;
using Njord.Ais.Interfaces;
using Njord.Server.Grains.States.Abstracts;

namespace Njord.Server.Grains.States
{
    [GenerateSerializer]
    public sealed record VesselState : AbstractMovingPositionState
    {
        [Id(0)]
        public sbyte RateOfTurn { get; set; }
        [Id(1)]
        public SpecialManoeuvreIndicator SpecialManoeuvreIndicator { get; set; } = SpecialManoeuvreIndicator.NotAvailable;
        [Id(2)]
        public int TrueHeading { get; set; } = TrueHeadingExtensions.TrueHeadingNotAvailable;
        [Id(3)]
        public NavigationalStatus NavigationalStatus { get; set; } = NavigationalStatus.Undefined;
        [Id(4)]
        public string? Name { get; set; }
        [Id(5)]
        public string? CallSign { get; set; }
        [Id(6)]
        public string? Destination { get; set; }
        [Id(7)]
        public uint IMONumber { get; set; } = 0;
        [Id(8)]
        public float MaximumPresentStaticDraught { get; set; } = 0;
        [Id(9)]
        public IEstimatedTimeOfArrival? EstimatedTimeOfArrival { get;  set; }
        [Id(10)]
        public IDimensions? Dimensions { get; internal set; }
        [Id(11)]
        public TypeOfShipAndCargoType TypeOfShipAndCargoType { get;  set; } = TypeOfShipAndCargoType.NotAvailable;
    }
}
