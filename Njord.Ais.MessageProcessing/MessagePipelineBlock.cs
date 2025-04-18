namespace Njord.Ais.MessageProcessing
{
    public sealed record MessagePipelineBlock
    {
        public required MessageBlockType BlockType { get; init; }

        public required Type? InstanceType { get; init; }

        public required Type? InputType { get; init; }

        public required Type? OutputType { get; init; }

        public int? BufferSize { get; init; }

        public required string Name { get; init; }

        public IEnumerable<string>? Outputs { get; init; }
    }
}
