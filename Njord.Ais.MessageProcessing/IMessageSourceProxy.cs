namespace Njord.Ais.MessageProcessing
{
    public interface IMessageSourceProxy<T>
    {
        public void SetReceiver(Func<T, CancellationToken, Task<bool>> receiver);
        public Task ReceiveAsync(T message, CancellationToken token);
    }
}
