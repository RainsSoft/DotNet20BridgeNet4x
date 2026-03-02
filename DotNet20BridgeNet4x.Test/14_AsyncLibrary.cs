using System.Threading;
using System.Threading.Tasks;

namespace Concurrency
{
    public class AsyncLibrary : Example
    {
        public override int Id => 14;

        public override async Task RunAsync()
        {
            SynchronizationContext.SetSynchronizationContext(new NaiveSingleThreadSynchronizationContext());
            PrintWithThread($"1 - {SynchronizationContext.Current?.GetType()}");

            SomeLibraryCallAsync().Wait();
        }

        public async Task SomeLibraryCallAsync()
        {
            var task1 = SomeInternalLibraryCall1Async();
            var task2 = SomeInternalLibraryCall2Async();

            Thread.Sleep(100); // Some CPU-bound operations.

            await task1.ConfigureAwait(false);

            PrintWithThread($"2 - {SynchronizationContext.Current?.GetType()}");

            await task2.ConfigureAwait(false);

            PrintWithThread($"3 - {SynchronizationContext.Current?.GetType()}");

            await ThirdMethodAsync(); // We already called ConfigureAwait(false) above, right?..

            // Some heavy operation needed to be run on the thread pool.
            PrintWithThread($"4 - {SynchronizationContext.Current?.GetType()}");
        }

        public Task<string> SomeInternalLibraryCall1Async()
        {
            return Task.FromResult("value");
        }

        public async Task SomeInternalLibraryCall2Async()
        {
            PrintWithThread($"Internal 2 start - {SynchronizationContext.Current?.GetType()}");

            await Task.Delay(1).ConfigureAwait(false);

            PrintWithThread($"Internal 2 end - {SynchronizationContext.Current?.GetType()}");
        }

        public async Task ThirdMethodAsync()
        {
            PrintWithThread($"Internal 3 start - {SynchronizationContext.Current?.GetType()}");

            await Task.Delay(1000).ConfigureAwait(false);

            PrintWithThread($"Internal 3 end - {SynchronizationContext.Current?.GetType()}");
        }
    }
}
