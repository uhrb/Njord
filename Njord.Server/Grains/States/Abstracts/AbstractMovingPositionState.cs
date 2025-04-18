using Njord.Ais.Extensions.Interfaces;

namespace Njord.Server.Grains.States.Abstracts
{
    [GenerateSerializer]
    [Alias("Njord.Server.Grains.States.Abstracts.AbstractMovingPositionState")]
    public record AbstractMovingPositionState : AbstractPositionState
    {
        [Id(0)]
        public double CourseOverGround { get; set; } = SpeedAndCourseOverGroundExtensions.CourseNotAvailable;
        [Id(1)]
        public double SpeedOverGround { get; set; } = SpeedAndCourseOverGroundExtensions.SpeedNotAvailable;
    }
}
