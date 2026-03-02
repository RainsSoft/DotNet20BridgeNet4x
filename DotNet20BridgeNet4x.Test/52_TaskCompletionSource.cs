using System.Threading;
using System.Threading.Tasks;

namespace Concurrency
{
    public class TaskCompletionSourceExample : Example
    {
        public override int Id => 52;
        public override async Task RunAsync()
        {
            PrintWithThread("Main method");

            var tcs = new TaskCompletionSource();

            _ = Task.Run(async () =>
            {
                PrintWithThread("background thread start");

                await Task.Delay(500);

                PrintWithThread("setting result of tcs");
                tcs.SetResult();

                PrintWithThread("set result, waiting for another 500 ms");

                await Task.Delay(500);

                PrintWithThread("exiting background thread");
            });

            PrintWithThread("Starting awaiting tcs");
            //tcs.Task.Wait();
            await tcs.Task;

            PrintWithThread("Doing stuff in the main thread");
            Thread.Sleep(2000);

            PrintWithThread("The end of RunAsync method");
        }
    }
}
