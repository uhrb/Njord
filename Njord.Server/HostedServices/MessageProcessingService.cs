using Njord.Ais.Interfaces;
using Njord.Ais.MessageProcessing;
using Njord.Ais.MessageProcessing.Sinks;
using Njord.AisStream;
using Njord.Server.Intstrumentation;

namespace Njord.Server.HostedServices
{
    public class MessageProcessingService : BackgroundService
    {
        private readonly ILogger<MessageProcessingService> _logger;
        private readonly IDataflowPipelineBuilder _builder;

        public MessageProcessingService(ILogger<MessageProcessingService> logger, IDataflowPipelineBuilder builder)
        {
            _logger = logger;
            _builder = builder;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var pipeline = new MessagePipelineConfiguration
            {
                Name = "Simple pipeline",
                Blocks = [
                    new MessagePipelineBlock {
                        Name = "AisStreamSource",
                        InstanceType = typeof(AisStreamMessageSourceProxy),
                        InputType = null,
                        OutputType = typeof(RawAisMessage),
                        BlockType = MessageBlockType.Source,
                        Outputs = [
                            "AisStreamTransformer"
                        ]
                    },
                    new MessagePipelineBlock {
                        Name = "AisStreamTransformer",
                        InstanceType = typeof(AisStreamMessageTransformer),
                        InputType = typeof(RawAisMessage),
                        OutputType = typeof(IMessageId),
                        BlockType = MessageBlockType.Transformer,
                        Outputs = [
                            "OrleansSink"
                        ]
                    },
                    new MessagePipelineBlock {
                        Name = "OrleansSink",
                        BlockType = MessageBlockType.Sink,
                        InputType = typeof(IMessageId),
                        OutputType = null,
                        InstanceType = typeof(OrleansSink),
                        Outputs= null
                    }
                ]
            };

            _logger.LogInformation("Service starting");
            var builded = _builder.Build(pipeline, stoppingToken);
            await builded.ArmSourcesAsync();
            await builded.Completion();
            _logger.LogInformation("Service exiting");
        }
    }
}
