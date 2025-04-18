using Njord.Ais.SerDe.JSON;
using System.Text.Json.Serialization;

namespace Njord.AisStream
{
    public record AisStreamMetadata
    {
        /// <summary>
        /// The Maritime Mobile Service Identity (MMSI) is a unique nine-digit number used to identify a ship's radio communications. 
        /// </summary>
        [JsonPropertyName("MMSI")]
        public required int MMSI { get; init; }

        /// <summary>
        /// The name of the ship. This property uses a custom JSON converter 
        /// </summary>
        [JsonPropertyName("ShipName"), JsonConverter(typeof(JsonStringWithTrimConverter))]
        public string? ShipName { get; init; }

        /// <summary>
        /// Long
        /// </summary>
        [JsonPropertyName("longitude")]
        public required double Longitude { get; init; }

        /// <summary>
        /// Lat
        /// </summary>
        [JsonPropertyName("latitude")]
        public required double Latitude { get; init; }

        /// <summary>
        /// The UTC time when the AIS data was recorded as 
        /// </summary>
        [JsonPropertyName("time_utc")]
        public required string TimeUTC { get; init; }
    }

}
