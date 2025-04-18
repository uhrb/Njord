using Njord.Ais.Interfaces;
using System.Text.Json.Serialization;

namespace Njord.AisStream.ModelTypes
{
    public sealed record GeoArea : IGeoArea
    {
        [JsonPropertyName("Longitude1")]
        public double LongitudeLeftUp { get; init; }

        [JsonPropertyName("Latitude1")]
        public double LatitudeLeftUp { get; init; }

        [JsonPropertyName("Longitude2")]
        public double LongitudeRightDown { get; init; }

        [JsonPropertyName("Latitude2")]
        public double LatitudeRightDown { get; init; }
    }
}
