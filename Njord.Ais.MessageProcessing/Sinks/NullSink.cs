namespace Njord.Ais.MessageProcessing.Sinks
{
    public class NullSink<T> : IMessageSink<T>
    {
        public Task PutAsync(T message, CancellationToken token)
        {
            // nothing
            return Task.CompletedTask;
        }
    }
}
