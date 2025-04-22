using DuckDB.NET.Data;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Njord.Ais.MessageProcessing;
using Njord.Ais.MessageProcessing.Sinks;
using Njord.AisStream;
using Njord.NCA;

namespace Njord.MessageCollector
{
    public class MessageProcessor : BackgroundService
    {
        private readonly ILogger<MessageProcessor> _logger;
        private readonly IDataflowPipelineBuilder _builder;

        public MessageProcessor(ILogger<MessageProcessor> logger, IDataflowPipelineBuilder builder)
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
                            "RawBuffer"
                        ]
                    },
                    new MessagePipelineBlock {
                        Name = "NcaStreamSource",
                        InstanceType = typeof(NcaStreamRawMessageSourceProxy),
                        InputType = null,
                        OutputType = typeof(RawAisMessage),
                        BlockType = MessageBlockType.Source,
                        Outputs = [
                            "RawBuffer"
                        ]
                    },
                    new MessagePipelineBlock {
                        Name = "RawBuffer",
                        InstanceType = null,
                        InputType = typeof(RawAisMessage),
                        OutputType = typeof(RawAisMessage),
                        BlockType = MessageBlockType.Buffer,
                        Outputs = [
                            "RawBroadcast"
                        ]
                    },
                    new MessagePipelineBlock {
                        Name = "RawBroadcast",
                        InstanceType = null,
                        InputType = typeof(RawAisMessage),
                        OutputType = typeof(RawAisMessage),
                        BlockType = MessageBlockType.GuranteedBroadcast,
                        Outputs = [
                           // "StringCategoryMapping",
                            "LogSinkRaw",
                        ]
                    },
                    new MessagePipelineBlock {
                        Name = "LogSinkRaw",
                        InstanceType = typeof(InformationalLogMessageSink),
                        InputType = typeof(RawAisMessage),
                        OutputType = null,
                        BlockType = MessageBlockType.Sink,
                    }, /*
                    new MessagePipelineBlock {
                        Name = "StringCategoryMapping",
                        InstanceType= typeof(StringCategoryMappingSink),
                        InputType = typeof(RawAisMessage),
                        OutputType = null,
                        BlockType = MessageBlockType.Sink,
                    }
                    */
                ]
            };
            _logger.LogInformation("Dataflow service starting");
            var builded = _builder.Build(pipeline, stoppingToken);
            await builded.ArmSourcesAsync();
            await builded.Completion();
            _logger.LogInformation("Dataflow service exiting");
        }
    }
}
