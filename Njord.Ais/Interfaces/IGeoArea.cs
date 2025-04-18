namespace Njord.Ais.Interfaces
{
    public interface IGeoArea
    {
        public double LongitudeLeftUp { get; init; }
        public double LatitudeLeftUp { get; init; }
        public double LongitudeRightDown { get; init; }
        public double LatitudeRightDown { get; init; }
    }
}
