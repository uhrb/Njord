using Njord.Ais.Enums;
using Njord.Ais.Interfaces;

namespace Njord.Ais.Messages
{
    public interface ISingleSlotBinaryMessage : IUserId, IMessageId, IRepeatIndicator, IDestinationId
    {
        /// <summary>
        /// True if addressed, otherwise its broadcast
        /// </summary>
        public bool IsAddressed { get; init; }

        /// <summary>
        /// Binary data
        /// </summary>
        public ReadOnlyMemory<byte> Data { get; init; }

        /// <summary>
        /// True, if binary data is ApplicationIdentifier binary data, otherwise - data is unstructured
        /// </summary>
        public bool IsApplicationIdentifierEncodedData { get; init; }

        /// <summary>
        /// Application identifier
        /// </summary>
        public IApplicationIdentifier? ApplicationIdentifier { get; init; }

    }
}
