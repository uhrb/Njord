
namespace Njord.Ais.MessageProcessing
{
    public interface IDataflowPipelineBuilder
    {
        BuildedPipeline Build(MessagePipelineConfiguration pipeline, CancellationToken pipelineCancelation);
    }
}