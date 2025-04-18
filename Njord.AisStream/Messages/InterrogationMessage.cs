using Njord.Ais.Enums;
using Njord.Ais.Interfaces;
using Njord.Ais.Messages;
using Njord.AisStream.MessageConverters;
using System.Text.Json.Serialization;

namespace Njord.AisStream.Messages
{
    [JsonConverter(typeof(JsonInterrogationMessageConverter))]
    public sealed record InterrogationMessage : IInterrogationMessage
    {
        public required IEnumerable<IInterrogationWithDestination> Interrogations { get; init; }

        public required string UserId { get; init; }

        public required AisMessageType MessageId { get; init; }

        public required RepeatIndicator RepeatIndicator { get; init; }
    }
}
