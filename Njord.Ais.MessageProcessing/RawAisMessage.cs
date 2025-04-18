using System.Buffers;

namespace Njord.Ais.MessageProcessing
{
    public readonly struct RawAisMessage
    {
        public required ReadOnlyMemory<byte> RawData { get; init; }

        public required string MessageFormat { get; init; }
    }
}
