using System;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Concurrency
{
    public class MySynchronizationContext : SynchronizationContext
    {
        public override void Post(SendOrPostCallback d, object state)
        {
            SendOrPostCallback callbackAndRestoreContext = (obj) =>
            {
                SynchronizationContext.SetSynchronizationContext(this);

                d?.Invoke(obj);
            };

            base.Post(callbackAndRestoreContext, state);
        }
    }

    public class SynchronizationContextExample : Example
    {
        public override int Id => 13;

        public override async Task RunAsync()
        {
            var previousContext = SynchronizationContext.Current;
            SynchronizationContext.SetSynchronizationContext(new MySynchronizationContext());

            PrintWithThread("main method");
            PrintWithThread($"in main - {SynchronizationContext.Current?.GetType()}");

            await Task.Delay(100);
            //await Task.Delay(100).ConfigureAwait(false);

            PrintWithThread($"in main after await - {SynchronizationContext.Current?.GetType()}");

            /*Action a = () =>
            {
                PrintWithThread($"inside custom awaiter - {SynchronizationContext.Current?.GetType()}");
            };
            await a;*/

            await Task.Run(() =>
            {
                // Synchronization context doesn't flow by MSCORLIB code (internal ExecutionContext method).
                PrintWithThread($"in task - {SynchronizationContext.Current?.GetType()}");
            }).ContinueWith(task =>
            {
                PrintWithThread($"in continuewith - {SynchronizationContext.Current?.GetType()}");
            }).ContinueWith(task =>
            {
                PrintWithThread($"should run in UI thread - {SynchronizationContext.Current?.GetType()}");
            }, TaskScheduler.FromCurrentSynchronizationContext());

            PrintWithThread($"in main after second await - {SynchronizationContext.Current?.GetType()}");

            await Task.Delay(100).ConfigureAwait(false);

            PrintWithThread($"after configureawait false - {SynchronizationContext.Current?.GetType()}");
        }
    }

    ///

    public sealed class NaiveSingleThreadSynchronizationContext : SynchronizationContext
    {
        private readonly ConcurrentQueue<Action> _queue
            = new ConcurrentQueue<Action>();
        private readonly Task _eventLoop;

        public NaiveSingleThreadSynchronizationContext()
        {
            _eventLoop = Task.Run(() =>
            {
                while (true)
                {
                    if (_queue.TryDequeue(out var action))
                        action();

                    Thread.Sleep(10);
                }
            });
        }

        public override void Post(SendOrPostCallback d, object state)
        {
            Action action = () =>
            {
                SynchronizationContext.SetSynchronizationContext(this);

                d?.Invoke(state);
            };

            _queue.Enqueue(action);
        }
    }








    public static class AwaitExtensions
    {
        public static AwaiterIntegerMultiplier GetAwaiter(this Action action)
        {
            return new AwaiterIntegerMultiplier(action);
        }
    }

    public struct AwaiterIntegerMultiplier : INotifyCompletion
    {
        private readonly Action _action;

        public AwaiterIntegerMultiplier(Action action)
        {
            _action = action;
            IsCompleted = false;
        }

        public bool IsCompleted { get; private set; }

        public void OnCompleted(Action continuation)
        {
            if (continuation != null)
            {
                continuation();
                //Task.Run(continuation);
            }
        }

        public void GetResult()
        {
            _action();
            IsCompleted = true;
        }
    }
}
