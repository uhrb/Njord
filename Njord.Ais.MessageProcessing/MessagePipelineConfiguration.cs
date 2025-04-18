using System.Text.Json.Serialization;

namespace Njord.Ais.MessageProcessing
{
    public sealed record MessagePipelineConfiguration
    {
        [JsonPropertyName("Name")]
        public required string Name { get; init; }

        [JsonPropertyName("Blocks")]
        public required IEnumerable<MessagePipelineBlock> Blocks {get; init;}
    }
}
