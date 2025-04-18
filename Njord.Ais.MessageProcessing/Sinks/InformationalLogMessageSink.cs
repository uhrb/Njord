using Microsoft.Extensions.Logging;
using System.Text;

namespace Njord.Ais.MessageProcessing.Sinks
{
    public sealed class InformationalLogMessageSink: IMessageSink<RawAisMessage>
    {
        private readonly ILogger<InformationalLogMessageSink> _logger;

        public InformationalLogMessageSink(ILogger<InformationalLogMessageSink> logger)
        {
            _logger = logger;
        }
        public Task PutAsync(RawAisMessage message, CancellationToken token)
        {
            var msg = Encoding.UTF8.GetString(message.RawData.Span);
            _logger.LogInformation("{msg}", msg);
            return Task.CompletedTask;  
        }
    }
}
