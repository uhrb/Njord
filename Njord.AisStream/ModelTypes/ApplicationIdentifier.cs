using Njord.Ais.Interfaces;
using System.Text.Json.Serialization;

namespace Njord.AisStream.ModelTypes
{
    public record ApplicationIdentifier : IApplicationIdentifier
    {
        [JsonPropertyName("DesignatedAreaCode")]
        public required ushort DesignatedAreaCode { get; init; }

        [JsonPropertyName("FunctionIdentifier")]
        public required byte FunctionIdentifier { get; init; }
    }
}
