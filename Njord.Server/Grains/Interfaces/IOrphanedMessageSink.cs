using Njord.Ais.Interfaces;
using Orleans;

namespace Njord.Server.Grains.Interfaces
{
    [Alias("Njord.Server.Grains.IOrphanedMessageCollectorGrain")]
    public interface IOrphanedMessageSink : IGrainWithStringKey
    {
        Task ProcessOrhpanedMessage(string callerGrain, IMessageId messageId);
    }
}
