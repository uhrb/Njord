namespace Njord.Ais.Interfaces
{
    public interface ILongitudeAndLatitude
    {
        /// <summary>
        /// Logitude. For simplicity it is in decimal format. Originaly in AIS message it is in 1/10 000 min 
        /// (±180°, East = positive (as per 2’s complement), West = negative (as per 2’s complement).
        /// 181 = (6791AC0h) = not available = default).
        /// </summary>
        public double Longitude { get; init; }

        /// <summary>
        /// Latitude. For simplicity in decimal format. Originaly Latitude in 1/10 000 min 
        /// (±90°, North = positive (as per 2’s complement), South = negative (as per 2’s complement). 
        /// 91° (3412140h) = not available = default).
        /// </summary>
        public double Latitude { get; init; }
    }
}
