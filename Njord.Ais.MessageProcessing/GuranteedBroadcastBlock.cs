using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace Njord.Ais.MessageProcessing
{
    public class GuranteedBroadcastBlock<T> : IPropagatorBlock<T, T>
    {

        private readonly object _locker = new object();
        private readonly Func<T, T> _cloningFunction;
        private readonly CancellationToken _cancellationToken;
        private readonly ITargetBlock<T> _actionBlock;
        private readonly List<Subscription> _subscriptions = [];
        private readonly Task _completion;
        private CancellationTokenSource? _faultCTS = new();

        public GuranteedBroadcastBlock(Func<T, T> cloningFunction, DataflowBlockOptions? dataflowBlockOptions = null)
        {
            _cloningFunction = cloningFunction ?? throw new ArgumentNullException(nameof(cloningFunction));
            dataflowBlockOptions ??= new();
            _cancellationToken = dataflowBlockOptions.CancellationToken;

            _actionBlock = new ActionBlock<T>(async item =>
            {
                Task sendAsyncToAll;
                lock (_locker)
                {
                    var allSendAsyncTasks = _subscriptions.Select(sub => sub.Target.SendAsync(_cloningFunction(item), sub.CancellationSource.Token));
                    sendAsyncToAll = Task.WhenAll(allSendAsyncTasks);
                }
                await sendAsyncToAll;
            }, new ExecutionDataflowBlockOptions()
            {
                CancellationToken = dataflowBlockOptions.CancellationToken,
                BoundedCapacity = dataflowBlockOptions.BoundedCapacity,
                MaxMessagesPerTask = dataflowBlockOptions.MaxMessagesPerTask,
                TaskScheduler = dataflowBlockOptions.TaskScheduler,
                MaxDegreeOfParallelism = -1
            });

            var afterCompletion = _actionBlock.Completion.ContinueWith(t =>
            {
                lock (_locker)
                {
                    // PropagateCompletion
                    foreach (var subscription in _subscriptions)
                    {
                        if (subscription.PropagateCompletion)
                        {
                            if (t.IsFaulted)
                                subscription.Target.Fault(t.Exception);
                            else
                                subscription.Target.Complete();
                        }
                    }
                    // Cleanup
                    foreach (var subscription in _subscriptions)
                    {
                        subscription.CancellationSource.Dispose();
                    }
                    _subscriptions.Clear();
                    _faultCTS.Dispose();
                    _faultCTS = null; // Prevent future subscriptions to occur
                }
            }, TaskScheduler.Default);

            // Ensure that any exception in the continuation will be surfaced
            _completion = Task.WhenAll(_actionBlock.Completion, afterCompletion);
        }


        public Task Completion => _completion;

        public void Complete()
        {
            _actionBlock.Complete();
        }

        public T? ConsumeMessage(DataflowMessageHeader messageHeader, ITargetBlock<T> target, out bool messageConsumed)
        {
            throw new NotImplementedException();
        }

        public void Fault(Exception exception)
        {
            _actionBlock.Fault(exception);
            lock (_locker) _faultCTS?.Cancel();
        }

        public IDisposable LinkTo(ITargetBlock<T> target, DataflowLinkOptions linkOptions)
        {
            if (linkOptions.MaxMessages != DataflowBlockOptions.Unbounded)
                throw new NotSupportedException();
            Subscription subscription;
            lock (_locker)
            {
                if (_faultCTS == null) return new Unlinker(null); // Has completed
                var cancellationSource = CancellationTokenSource
                    .CreateLinkedTokenSource(_cancellationToken, _faultCTS.Token);
                subscription = new Subscription(target,
                    linkOptions.PropagateCompletion, cancellationSource);
                _subscriptions.Add(subscription);
            }
            return new Unlinker(() =>
            {
                lock (_locker)
                {
                    // The subscription may have already been removed
                    if (_subscriptions.Remove(subscription))
                    {
                        subscription.CancellationSource.Cancel();
                        subscription.CancellationSource.Dispose();
                    }
                }
            });
        }

        public DataflowMessageStatus OfferMessage(DataflowMessageHeader messageHeader, T messageValue, ISourceBlock<T>? source, bool consumeToAccept)
        {
            return _actionBlock.OfferMessage(messageHeader, messageValue, source, consumeToAccept);
        }

        public void ReleaseReservation(DataflowMessageHeader messageHeader, ITargetBlock<T> target)
        {
            throw new NotSupportedException();
        }

        public bool ReserveMessage(DataflowMessageHeader messageHeader, ITargetBlock<T> target)
        {
            throw new NotSupportedException();
        }

        private class Subscription
        {
            public readonly ITargetBlock<T> Target;
            public readonly bool PropagateCompletion;
            public readonly CancellationTokenSource CancellationSource;

            public Subscription(ITargetBlock<T> target,
                bool propagateCompletion,
                CancellationTokenSource cancellationSource)
            {
                Target = target;
                PropagateCompletion = propagateCompletion;
                CancellationSource = cancellationSource;
            }
        }

        private class Unlinker : IDisposable
        {
            private readonly Action? _action;
            public Unlinker(Action? disposeAction) => _action = disposeAction;
            void IDisposable.Dispose() => _action?.Invoke();
        }
    }
}
