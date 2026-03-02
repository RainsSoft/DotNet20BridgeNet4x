using System;
using System.Threading;
using System.Threading.Tasks;

namespace Concurrency
{
    public class Cancellation : Example
    {
        public override int Id => 8;

        public override async Task RunAsync()
        {
            PrintWithThread("main method");

            using var cts = new CancellationTokenSource();

            var task = DoSmthAsync(cts.Token);
            //var task = DoSmthAsync_2(cts.Token);
            //var task = DoSmthAsync_3(cts.Token);

            PrintWithThread("task acquired");

            await Task.Delay(100);

            PrintWithThread("after first await");

            cts.Cancel();

            try
            {
                await task;
            }
            catch (OperationCanceledException)
            {
                PrintWithThread($"Task has been canceled: {task.Status}");
            }

            PrintWithThread("the end");
        }

        public async Task DoSmthAsync(CancellationToken cancellationToken)
        {
            PrintWithThread("Entered DoSmthAsync method.");

            await Task.Delay(2000, cancellationToken);

            PrintWithThread("After delay");
        }

        public async Task DoSmthAsync_2(CancellationToken cancellationToken)
        {
            PrintWithThread("Entered DoSmthAsync_2 method.");

            try
            {
                await Task.Delay(2000, cancellationToken);
            }
            catch (OperationCanceledException)
            {
                // Some cleanup process.
                await Task.Delay(200);
                throw;
            }
        }

        public async Task DoSmthAsync_3(CancellationToken cancellationToken)
        {
            PrintWithThread("Entered DoSmthAsync_3 method.");

            await Task.Delay(2000);

            cancellationToken.ThrowIfCancellationRequested();
        }
    }
}
