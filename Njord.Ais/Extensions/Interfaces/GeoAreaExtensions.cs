using Njord.Ais.Interfaces;

namespace Njord.Ais.Extensions.Interfaces
{
    public static class GeoAreaExtensions
    {
        public static bool IsValid(this IGeoArea geoArea)
        {
            return geoArea.LongitudeLeftUp >= -180
                && geoArea.LongitudeLeftUp <= 181
                && geoArea.LongitudeRightDown >= -180
                && geoArea.LongitudeRightDown <= 181
                && geoArea.LatitudeLeftUp >= -90
                && geoArea.LatitudeLeftUp <= 91
                && geoArea.LatitudeRightDown >= -90
                && geoArea.LatitudeRightDown <= 91;
        }
    }
}
