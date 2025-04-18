namespace Njord.Ais.MessageProcessing
{
    public interface IMessageSink<T>
    {
        Task PutAsync(T message, CancellationToken token);  
    }
}
