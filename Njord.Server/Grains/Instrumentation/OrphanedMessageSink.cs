using Njord.Ais.Interfaces;
using Njord.Server.Grains.Interfaces;
using Orleans;

namespace Njord.Server.Grains.Instrumentation
{
    public class OrphanedMessageSink : Grain, IOrphanedMessageGrain
    {
        public const string OrphanedMessageSinkGrainKey = nameof(OrphanedMessageSink);

        public Task ProcessOrhpanedMessage(string callerGrain, IMessageId messageId)
        {
            return Task.CompletedTask;
        }
    }
}
