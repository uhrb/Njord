using Njord.Ais.Interfaces;
using Njord.Server.Grains.Interfaces;
using Orleans;

namespace Njord.Server.Grains.Instrumentation
{
    public class FailedMessageSink : Grain, IFailedMessageGrain
    {
        public const string FailedMessageSinkGrainKey = nameof(FailedMessageSink);

        public Task ProcessFailedMessage(string callerGrain, IMessageId messageId)
        {
            return Task.CompletedTask;
        }
    }
}
