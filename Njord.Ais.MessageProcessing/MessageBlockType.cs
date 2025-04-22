namespace Njord.Ais.MessageProcessing
{
    public enum MessageBlockType
    {
        Source,
        Sink,
        Transformer,
        Broadcast,
        Join,
        GuranteedBroadcast,
        Buffer
    }
}
