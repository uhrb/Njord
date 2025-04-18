using Microsoft.Extensions.Logging;
using Njord.Ais.Interfaces;
using Njord.Ais.MessageProcessing;
using System.Text;
using System.Text.Json;

namespace Njord.AisStream
{
    public sealed class AisStreamMessageTransformer : IMessageTransformer<RawAisMessage, IMessageId>
    {
        private readonly ILogger<AisStreamMessageTransformer> _logger;

        public AisStreamMessageTransformer(ILogger<AisStreamMessageTransformer> logger)
        {
            _logger = logger;
        }
        public Task<IEnumerable<IMessageId>> TransformAsync(RawAisMessage message, CancellationToken token)
        {
            AisStreamEnvelope? envelope = null;
            try
            {
                envelope = JsonSerializer.Deserialize<AisStreamEnvelope>(message.RawData.Span);

            }
            catch (Exception ex)
            {
                var msg = Encoding.UTF8.GetString(message.RawData.Span);
                _logger.LogError(ex, "Cant deserialize message {MessageFormat}: {msg}", message.MessageFormat, msg);
            }

            if (envelope != null && envelope.MessageType != AisStreamMessageType.UnknownMessage)
            {
                return Task.FromResult(Enumerable.Repeat(envelope.Message!, 1));
            }

            return Task.FromResult(Enumerable.Empty<IMessageId>());
        }
    }
}
