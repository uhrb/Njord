namespace Njord.Ais.Interfaces
{
    public interface ISpeedAndCourseOverGround
    {
        /// <summary>
        /// Speed over ground in knots. Base AIS definition is 1/10 of step, however 
        /// for simplification it is replaced with more readable value.
        /// 102.3 = not available, 102.2 = 102.2 knots or higher
        /// </summary>
        public double SpeedOverGround { get; init; }

        /// <summary>
        /// Course over ground. For simplicity in decimal format. Originally Course over ground in 1/10 = (0-3 599). 
        /// 3 600 (E10h) = not available = default. 
        /// 3 601-4 095 should not be used
        /// </summary>
        public double CourseOverGround { get; init; }
    }
}
