using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.Metrics;
using System.Reflection;
using System.Threading.Tasks.Dataflow;

namespace Njord.Ais.MessageProcessing
{
    public sealed class DataflowPipelineBuilder : IDataflowPipelineBuilder
    {
        private readonly IServiceProvider _provider;
        private readonly Meter _meter;
        private readonly Counter<long> _messagesCount;

        public DataflowPipelineBuilder(IServiceProvider provider, IMeterFactory factory)
        {
            _provider = provider;
            _meter = factory.Create(typeof(DataflowPipelineBuilder).FullName!, "v1.0");
            _messagesCount = _meter.CreateCounter<long>("messagesCount");
        }

        public BuildedPipeline Build(MessagePipelineConfiguration pipeline, CancellationToken pipelineCancelation)
        {
            var sources = pipeline.Blocks.Where(_ => _.BlockType == MessageBlockType.Source).ToList();
            var transformers = pipeline.Blocks.Where(_ => _.BlockType == MessageBlockType.Transformer).ToList();
            var sinks = pipeline.Blocks.Where(_ => _.BlockType == MessageBlockType.Sink).ToList();

            var blockDicts = new Dictionary<string, IDataflowBlock>();
            var blockDefDicts = new Dictionary<string, MessagePipelineBlock>();

            var blockNamesExceptSources = new HashSet<string>(pipeline.Blocks.Where(_ => _.BlockType != MessageBlockType.Source).Select(_ => _.Name).Distinct());
            var linkNames = new HashSet<string>(pipeline.Blocks.SelectMany(_ => _.Outputs ?? []).Distinct());


            if (!blockNamesExceptSources.SetEquals(linkNames))
            {
                throw new DataMisalignedException("Pipeline contains orphaned elements");
            }

            var sourceArms = new List<Func<Task>>();


            // creating blocks
            foreach (var block in pipeline.Blocks)
            {
                if (blockDicts.ContainsKey(block.Name))
                {
                    continue;
                }

                blockDefDicts.Add(block.Name, block);

                switch (block.BlockType)
                {
                    case MessageBlockType.Source:
                        CreateGenericSource(blockDicts, sourceArms, block, pipeline.Name, pipelineCancelation);
                        break;

                    case MessageBlockType.Sink:
                        ConstructGenericSink(blockDicts, block, pipeline.Name, pipelineCancelation);
                        break;

                    case MessageBlockType.Transformer:
                        ConstructGenericTransformer(blockDicts, block, pipeline.Name, pipelineCancelation);
                        break;

                    case MessageBlockType.Broadcast:
                        ConstructGenericBroadcast(blockDicts, block, pipeline.Name, pipelineCancelation);
                        break;

                    case MessageBlockType.GuranteedBroadcast:
                        ConstructGenericGuranteedBroadcast(blockDicts, block, pipeline.Name, pipelineCancelation);
                        break;

                    case MessageBlockType.Join:
                        ConstructGenericJoin(blockDicts, block, pipelineCancelation);
                        break;

                    case MessageBlockType.Buffer:
                        ConstructGenericBuffer(blockDicts, block, pipelineCancelation);
                        break;

                    default:
                        throw new DataMisalignedException("Unknown block type");
                }
            }

            var staticLinkMethod = typeof(DataflowPipelineBuilder).GetMethod(nameof(LinkBlocks), BindingFlags.Static | BindingFlags.NonPublic)!;

            // linking blocks
            foreach (var currentBlock in pipeline.Blocks)
            {
                var currentInstance = blockDicts[currentBlock.Name];

                foreach (var targetName in currentBlock.Outputs ?? []) // skipping all the empty outputs -
                {
                    var targetInstance = blockDicts[targetName];
                    var targetDef = blockDefDicts[targetName];
                    if (targetDef.InputType != currentBlock.OutputType)
                    {
                        throw new DataMisalignedException("Target input type not the same as source output");
                    }

                    var desiredType = currentBlock.OutputType!; // since we checked

                    var generic = staticLinkMethod.MakeGenericMethod(desiredType)!;
                    generic.Invoke(this, [currentInstance, targetInstance]);
                }
            }

            return new BuildedPipeline
            {
                Completion = () => Completion(blockDicts.Values),
                ArmSourcesAsync = () => ArmSources(sourceArms)
            };
        }

        private void ConstructGenericJoin(
            Dictionary<string, IDataflowBlock> blockDicts,
            MessagePipelineBlock block,
            CancellationToken pipelineCancelation)
        {
            if (block.InputType == null || block.OutputType == null || block.InputType != block.OutputType)
            {
                throw new DataMisalignedException("Join block inputs and outpus should be defined and be the same");
            }
            var methodJoin = typeof(DataflowPipelineBuilder).GetMethod(nameof(ConstructJoin), BindingFlags.Static | BindingFlags.NonPublic)!;
            var genericMethodJoin = methodJoin.MakeGenericMethod(block.InputType);
            genericMethodJoin.Invoke(this, [block, blockDicts, pipelineCancelation]);
        }

        private void ConstructGenericBroadcast(
            Dictionary<string, IDataflowBlock> blockDicts,
            MessagePipelineBlock block,
            string pipelineName,
            CancellationToken pipelineCancelation)
        {
            if (block.InputType == null || block.OutputType == null || block.InputType != block.OutputType)
            {
                throw new DataMisalignedException("Broadcast block inputs and outpus should be defined and be the same");
            }
            var methodBroadcast = typeof(DataflowPipelineBuilder).GetMethod(nameof(ConstructBroadcast), BindingFlags.Static | BindingFlags.NonPublic)!;
            var genericMethodBroadcast = methodBroadcast.MakeGenericMethod(block.InputType);
            genericMethodBroadcast.Invoke(this, [block, blockDicts, _messagesCount, pipelineName, pipelineCancelation]);
        }

        private void ConstructGenericGuranteedBroadcast(
            Dictionary<string, IDataflowBlock> blockDicts,
            MessagePipelineBlock block,
            string pipelineName,
            CancellationToken pipelineCancelation)
        {
            if (block.InputType == null || block.OutputType == null || block.InputType != block.OutputType)
            {
                throw new DataMisalignedException("Broadcast block inputs and outpus should be defined and be the same");
            }
            var methodBroadcast = typeof(DataflowPipelineBuilder).GetMethod(nameof(ConstructGuranteedBroadcast), BindingFlags.Static | BindingFlags.NonPublic)!;
            var genericMethodBroadcast = methodBroadcast.MakeGenericMethod(block.InputType);
            genericMethodBroadcast.Invoke(this, [block, blockDicts, _messagesCount, pipelineName, pipelineCancelation]);
        }

        private void ConstructGenericTransformer(
            Dictionary<string, IDataflowBlock> blockDicts,
            MessagePipelineBlock block,
            string pipelineName,
            CancellationToken pipelineCancelation)
        {
            if (block.OutputType == null || block.InputType == null || block.InstanceType == null)
            {
                throw new DataMisalignedException("Transformer block should define input and output message format and instance type");
            }
            var methodTransform = typeof(DataflowPipelineBuilder).GetMethod(nameof(ConstructTransformer), BindingFlags.Static | BindingFlags.NonPublic)!;
            var genericMethodTransform = methodTransform.MakeGenericMethod(block.InstanceType, block.InputType, block.OutputType);
            genericMethodTransform.Invoke(this, [_provider, block, blockDicts, _messagesCount, pipelineName, pipelineCancelation]);
        }

        private void ConstructGenericSink(
            Dictionary<string, IDataflowBlock> blockDicts,
            MessagePipelineBlock block,
            string pipelineName,
            CancellationToken pipelineCancelation)
        {
            if (block.InputType == null || block.InstanceType == null)
            {
                throw new DataMisalignedException("Sink block should define input message format and instance type");
            }
            var methodSink = typeof(DataflowPipelineBuilder).GetMethod(nameof(ConstructSink), BindingFlags.Static | BindingFlags.NonPublic)!;
            var genericMethodSink = methodSink.MakeGenericMethod(block.InstanceType, block.InputType);
            genericMethodSink.Invoke(this, [_provider, block, blockDicts, _messagesCount, pipelineName, pipelineCancelation]);
        }

        private void CreateGenericSource(
            Dictionary<string, IDataflowBlock> blockDicts,
            List<Func<Task>> sourceArms,
            MessagePipelineBlock block,
            string pipelineName,
            CancellationToken pipelineCancelation)
        {
            if (block.OutputType == null || block.InstanceType == null)
            {
                throw new DataMisalignedException("Source block should define output message format and instance type");
            }
            var method = typeof(DataflowPipelineBuilder).GetMethod(nameof(ConstructSource), BindingFlags.Static | BindingFlags.NonPublic)!;
            var genericMethod = method.MakeGenericMethod(block.InstanceType, block.OutputType);
            genericMethod.Invoke(this, [_provider, block, blockDicts, sourceArms, _messagesCount, pipelineName, pipelineCancelation]);
        }

        private void ConstructGenericBuffer(Dictionary<string, IDataflowBlock> blockDicts, MessagePipelineBlock block, CancellationToken pipelineCancelation)
        {
            if (block.InputType == null || block.InstanceType != null || block.OutputType == null || block.InputType != block.OutputType)
            {
                throw new DataMisalignedException("Buffer block should have no instance type and input and output types should be equal");
            }
            var method = typeof(DataflowPipelineBuilder).GetMethod(nameof(ConstructBuffer), BindingFlags.Static | BindingFlags.NonPublic)!;
            var genericMethod = method.MakeGenericMethod(block.InputType);
            genericMethod.Invoke(this, [block, blockDicts, pipelineCancelation]);
        }

        private static void LinkBlocks<TMessage>(ISourceBlock<TMessage> source, ITargetBlock<TMessage> target)
        {
            source.LinkTo(target);
        }

        private static async Task ArmSources(IEnumerable<Func<Task>> wires)
        {
            foreach (var w in wires)
            {
                await w();
            }
        }

        private static async Task Completion(IEnumerable<IDataflowBlock> blocks)
        {
            foreach (var block in blocks)
            {
                try
                {
                    await block.Completion;
                }
                catch (TaskCanceledException)
                {
                }
            }
        }

        private static void ConstructBuffer<TMessage>(
            MessagePipelineBlock block,
            Dictionary<string, IDataflowBlock> blockDicts,
            CancellationToken pipelineCancelation)
        {
            if (block.Outputs?.Count() != 1)
            {
                throw new DataMisalignedException("Source interface need to gave exactly one output");
            }
           
            var blockInstance = new BufferBlock<TMessage>(new DataflowBlockOptions
            {
                CancellationToken = pipelineCancelation,
            })!;

            blockDicts.Add(block.Name, blockInstance);
        }

        private static void ConstructSource<TInstance, TMessage>(
            IServiceProvider provider,
            MessagePipelineBlock block,
            Dictionary<string, IDataflowBlock> blockDicts,
            List<Func<Task>> sourceArms,
            Counter<long> messagesCount,
            string pipelineName,
            CancellationToken pipelineCancelation)
        {
            if (block.Outputs?.Count() != 1)
            {
                throw new DataMisalignedException("Source interface need to gave exactly one output");
            }
            if (provider.GetService<TInstance>() is not IMessageSourceProxy<TMessage> sourceInstance)
            {
                throw new DataMisalignedException("Not able to find source type specified");
            }
            var blockInstance = new BufferBlock<TMessage>(new DataflowBlockOptions
            {
                CancellationToken = pipelineCancelation,
            })!;

            sourceArms.Add(() =>
            {
                sourceInstance.SetReceiver((_, __) =>
                {
                    messagesCount.Add(1, new("pipeline", pipelineName), new("blockName", block.Name), new("blockType", Enum.GetName(block.BlockType)));
                    return Task.FromResult(blockInstance.Post(_));
                });
                return Task.CompletedTask;
            });

            blockDicts.Add(block.Name, blockInstance);
        }

        private static void ConstructTransformer<TInstance, TMessage1, TMessage2>(
            IServiceProvider provider,
            MessagePipelineBlock block,
            Dictionary<string, IDataflowBlock> blockDicts,
            Counter<long> messagesCount,
            string pipelineName,
            CancellationToken pipelineCancelation)
        {
            if (block.Outputs?.Count() != 1)
            {
                throw new DataMisalignedException("Transformer block need to have exact one output");
            }
            if (provider.GetService<TInstance>() is not IMessageTransformer<TMessage1, TMessage2> sourceInstance)
            {
                throw new DataMisalignedException("Not able to find specified transformer type");
            }

            var blockInstance = new TransformManyBlock<TMessage1, TMessage2>(
                async _ =>
                {
                    messagesCount.Add(1, new("pipeline", pipelineName), new("blockName", block.Name));
                    return await sourceInstance.TransformAsync(_, pipelineCancelation);
                },
                new ExecutionDataflowBlockOptions
                {
                    CancellationToken = pipelineCancelation,
                    BoundedCapacity = block.BufferSize ?? -1,
                    MaxDegreeOfParallelism = -1
                });
            blockDicts.Add(block.Name, blockInstance);
        }

        private static void ConstructSink<TInstance, TMessage>(
            IServiceProvider provider,
            MessagePipelineBlock block,
            Dictionary<string, IDataflowBlock> blockDicts,
            Counter<long> messagesCount,
            string pipelineName,
            CancellationToken pipelineCancelation)
        {
            if (block.Outputs != null && block.Outputs.Any())
            {
                throw new DataMisalignedException("Sink cannot have outputs");
            }
            if (provider.GetService<TInstance>() is not IMessageSink<TMessage> sourceInstance)
            {
                throw new DataMisalignedException("Not able to find specified sink type");
            }
            var blockInstance = new ActionBlock<TMessage>(
                async _ =>
                {
                    messagesCount.Add(1, new("pipeline", pipelineName), new("blockName", block.Name), new("blockType", Enum.GetName(block.BlockType)));
                    await sourceInstance.PutAsync(_, pipelineCancelation);
                },
                new ExecutionDataflowBlockOptions
                {
                    CancellationToken = pipelineCancelation,
                    BoundedCapacity = block.BufferSize ?? -1,
                    MaxDegreeOfParallelism = -1
                });
            blockDicts.Add(block.Name, blockInstance);
        }


        private static void ConstructJoin<TMessage>(
            MessagePipelineBlock block,
            Dictionary<string, IDataflowBlock> blockDicts,
            CancellationToken pipelineCancelation)
        {
            if (block.Outputs?.Count() != 1)
            {
                throw new DataMisalignedException("Join interface need to have exactly one output");
            }
            var blockInstance = new JoinBlock<TMessage, TMessage>(
                new GroupingDataflowBlockOptions
                {
                    CancellationToken = pipelineCancelation,
                    BoundedCapacity = block.BufferSize ?? -1,
                });
            blockDicts.Add(block.Name, blockInstance);
        }

        private static void ConstructBroadcast<TMessage>(
            MessagePipelineBlock block,
            Dictionary<string, IDataflowBlock> blockDicts,
            Counter<long> messagesCount,
            string pipelineName,
            CancellationToken pipelineCancelation)
        {
            if (block.Outputs == null || !block.Outputs.Any())
            {
                throw new DataMisalignedException("Broadcast interface need to have at least one output");
            }
            var blockInstance = new BroadcastBlock<TMessage>(
                _ =>
                {
                    messagesCount.Add(1, new("pipeline", pipelineName), new("blockName", block.Name), new("blockType", Enum.GetName(block.BlockType)));
                    return _;
                },
                new DataflowBlockOptions
                {
                    CancellationToken = pipelineCancelation,
                    BoundedCapacity = block.BufferSize ?? -1,
                });
            blockDicts.Add(block.Name, blockInstance);
        }

        private static void ConstructGuranteedBroadcast<TMessage>(
           MessagePipelineBlock block,
           Dictionary<string, IDataflowBlock> blockDicts,
           Counter<long> messagesCount,
           string pipelineName,
           CancellationToken pipelineCancelation)
        {
            if (block.Outputs == null || !block.Outputs.Any())
            {
                throw new DataMisalignedException("Broadcast interface need to have at least one output");
            }
            var blockInstance = new GuranteedBroadcastBlock<TMessage>(
                _ =>
                {
                    messagesCount.Add(1, new("pipeline", pipelineName), new("blockName", block.Name), new("blockType", Enum.GetName(block.BlockType)));
                    return _;
                },
                new DataflowBlockOptions
                {
                    CancellationToken = pipelineCancelation,
                    BoundedCapacity = block.BufferSize ?? -1,
                });
            blockDicts.Add(block.Name, blockInstance);
        }
    }
}
