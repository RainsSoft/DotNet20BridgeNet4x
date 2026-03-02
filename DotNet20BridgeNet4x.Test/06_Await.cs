using System.Threading;
using System.Threading.Tasks;

namespace Concurrency
{
    public class Await : Example
    {
        public override int Id => 6;

        public override async Task RunAsync()
        {
            PrintWithThread("main method");

            await DoSmthAsync();

            PrintWithThread("second line");

            await Task.Delay(100);

            PrintWithThread("third line");

            await DoSmthAsync();

            PrintWithThread("The end");
        }

        private async Task DoSmthAsync()
        {
            PrintWithThread("Entered DoSmthAsync method.");

            Thread.Sleep(500);

            PrintWithThread("Finished sleeping for half a second.");

            await Task.Delay(100);

            PrintWithThread("After first await");

            await Task.Delay(100);

            PrintWithThread("After second await");
        }

        public async Task ConventionAsync(CancellationToken cancellationToken = default)
        {
            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();

                // TODO: Pass cancellationToken here.
                await Task.Delay(100); // Some work.
            }
        }
    }
}
