using Njord.Ais.Enums;
using Njord.Ais.Extensions.Interfaces;

namespace Njord.Server.Grains.States.Abstracts
{
    [GenerateSerializer]
    [Alias("Njord.Server.Grains.States.Abstracts.AbstractPositionState")]
    public abstract record AbstractPositionState
    {
        [Id(0)]
        public double Longitude { get; set; } = LongitudeAndLatitudeExtensions.LongitudeNotAvailable;
        [Id(1)]
        public double Latitude { get; set; } = LongitudeAndLatitudeExtensions.LatitudeNotAvailable;
        [Id(2)]
        public bool? IsPositionAccuracyHigh { get; set; }
        [Id(3)]
        public bool? IsRaimInUse { get; set; }
        [Id(4)]
        public PositionFixingDeviceType FixingDeviceType { get; set; } = PositionFixingDeviceType.Undefined;
        [Id(5)]
        public DateTime Updated { get; set; }
    }
}
