using Njord.Ais.Enums;
using Njord.Ais.Interfaces;
using Njord.Ais.Messages;
using Njord.Ais.SerDe.JSON;
using Njord.AisStream.ModelTypes;
using System.Text.Json.Serialization;

namespace Njord.AisStream.Messages
{
    public sealed record AddressedBinaryMessage : IAddressedBinaryMessage
    {
        [JsonPropertyName("ApplicationID"), JsonConverter(typeof(JsonInterfaceFactoryConverter<IApplicationIdentifier, ApplicationIdentifier>))]
        public required IApplicationIdentifier ApplicationIdentifier { get; init; }

        [JsonPropertyName("BinaryData"), JsonConverter(typeof(JsonStringToBinaryDataConverter))]
        public required ReadOnlyMemory<byte> ApplicationData { get; init; }

        [JsonPropertyName("UserID"), JsonConverter(typeof(JsonIntToMMSIStringConverter))]
        public required string UserId { get; init; }

        [JsonPropertyName("MessageID"), JsonConverter(typeof(JsonCheckedNumberEnumConverter<AisMessageType>))]
        public required AisMessageType MessageId { get; init; }

        [JsonPropertyName("RepeatIndicator"), JsonConverter(typeof(JsonCheckedNumberEnumConverter<RepeatIndicator>))]
        public required RepeatIndicator RepeatIndicator { get; init; }

        [JsonPropertyName("Sequenceinteger")]
        public required byte SequenceNumber { get; init; }

        [JsonPropertyName("DestinationID"), JsonConverter(typeof(JsonIntToMMSIStringConverter))]
        public required string DestinationId { get; init; }

        [JsonPropertyName("Retransmission")]
        public required bool IsRetransmitted { get; init; }
    }
}
