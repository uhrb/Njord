using Njord.Ais.Interfaces;
using Orleans;

namespace Njord.Server.Grains.Interfaces
{
    [Alias("Njord.Server.Grains.Interfaces.IFailedMessageSinkGrain")]
    public interface IFailedMessageSink : IGrainWithStringKey
    {
        public Task ProcessFailedMessage(string callerGrain, IMessageId messageId);
    }
}
