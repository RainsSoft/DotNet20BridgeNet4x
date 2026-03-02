using System;
using System.Threading;
using System.Threading.Tasks;

namespace Concurrency
{
    public class CombinedCancellation : Example
    {
        public override int Id => 9;

        public override async Task RunAsync()
        {
            PrintWithThread("main method");

            using var cts = new CancellationTokenSource();

            try
            {
                await DoSmthAsync(cts.Token);
            }
            catch (OperationCanceledException)
            {
                PrintWithThread("task is canceled");
            }
        }

        public async Task DoSmthAsync(CancellationToken cancellationToken)
        {
            using var cts = new CancellationTokenSource(500);
            using var combinedCts = CancellationTokenSource.CreateLinkedTokenSource(
                cts.Token, cancellationToken);

            await Task.Delay(2000, combinedCts.Token);
        }
    }
}
