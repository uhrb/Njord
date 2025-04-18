namespace Njord.Ais.MessageProcessing
{
    public sealed record BuildedPipeline
    {
        public required Func<Task> Completion { get; init; }

        public required Func<Task> ArmSourcesAsync { get; init; }   
    }
}
