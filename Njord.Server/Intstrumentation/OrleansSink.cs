using Njord.Ais.Interfaces;
using Njord.Ais.MessageProcessing;
using Njord.Server.Grains.Instrumentation;

namespace Njord.Server.Intstrumentation
{
    public class OrleansSink : IMessageSink<IMessageId>
    {
        private readonly IGrainAccessor _accessor;
        private readonly ILogger<OrleansSink> _logger;

        public OrleansSink(IGrainAccessor accessor, ILogger<OrleansSink> logger)
        {
            _accessor = accessor;
            _logger = logger;
        }

        public Task PutAsync(IMessageId message, CancellationToken token)
        {
            var toUser = (IUserId)message;
            var grain = _accessor.GetGrainFromMMSI(toUser.UserId);
            if (grain == null)
            {
                _logger.LogDebug("No grain mapped for {UserId}", toUser.UserId);
            }
            else
            {
                _ = Task.Run(() => grain.ProcessMessage(message), token);
            }

            return Task.CompletedTask;
        }
    }
}
