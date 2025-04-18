using Njord.Ais.Interfaces;
using System.Text.Json.Serialization;

namespace Njord.AisStream.ModelTypes
{
    public sealed record Dimensions : IDimensions
    {
        [JsonPropertyName("A")]
        public required ushort A { get; init; }

        [JsonPropertyName("B")]
        public required ushort B { get; init; }

        [JsonPropertyName("C")]
        public required byte C { get; init; }

        [JsonPropertyName("D")] 
        public required byte D { get; init; }
    }
}
