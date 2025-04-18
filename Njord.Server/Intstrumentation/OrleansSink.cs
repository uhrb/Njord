using Njord.Ais.Interfaces;
using Njord.Ais.MessageProcessing;
using Njord.Server.Grains.Interfaces;

namespace Njord.Server.Intstrumentation
{
    public class OrleansSink : IMessageSink<IMessageId>
    {
        private readonly IGrainFactory _factory;

        public OrleansSink(IGrainFactory factory)
        {
            _factory = factory;
        }

        public async Task PutAsync(IMessageId message, CancellationToken token)
        {
            var toUser = (IUserId)message;

            var grain = _factory.GetGrain<IUnknownMaritimeEntity>(toUser.UserId);
            _ = Task.Run(() => grain.ProcessMessage(message));
        }
    }
}
