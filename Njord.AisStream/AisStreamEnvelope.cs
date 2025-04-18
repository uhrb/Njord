using Njord.Ais.Enums;
using Njord.Ais.Interfaces;
using Njord.AisStream.Converters;
using System.Text.Json.Serialization;

namespace Njord.AisStream
{
    /// <summary>
    /// Base class for envelope, received from AISSTREAM
    /// </summary>
    /// 
    [JsonConverter(typeof(JsonAisStreamEnvelopeConverter))]
    public record AisStreamEnvelope
    {
        /// <summary>
        /// Message itself. Do not access it directly
        /// </summary>
        public required IMessageId? Message { get; init; }

        /// <summary>
        /// Message type. <see cref="AisMessageType"/>
        /// </summary>
        public required AisStreamMessageType MessageType { get; init; }

        /// <summary>
        /// Assiciated metadata
        /// </summary>
        public required AisStreamMetadata Metadata { get; init; }
    }
}
