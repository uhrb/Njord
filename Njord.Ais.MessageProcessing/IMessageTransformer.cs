namespace Njord.Ais.MessageProcessing
{
    public interface IMessageTransformer<T1,T2>
    {
        public Task<IEnumerable<T2>> TransformAsync(T1 message, CancellationToken token);
    }
}
