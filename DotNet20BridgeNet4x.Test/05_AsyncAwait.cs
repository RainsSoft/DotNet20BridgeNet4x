using System;
using System.Threading;
using System.Threading.Tasks;

namespace Concurrency
{
    // There's a bug here.
    public class AsyncAwait : Example
    {
        public override int Id => 5;

        public override Task RunAsync()
        {
            PrintWithThread("main method");

            DoSmthAsync().Wait();
            //DoSmthAsync_2().Wait();
            //DoSmthAsync_3().Wait();
            //DoSmthAsync_4().Wait();
            //DoSmthAsync_5().Wait();

            //var task = DoSmthAsync();
            //task.Wait();

            return Task.CompletedTask;
        }

        private Task DoSmthAsync()
        {
            PrintWithThread("Entered DoSmthAsync method.");

            var task = Task.Run(() =>
            {
                PrintWithThread("First line");
            })
                .ContinueWith(task => Task.Delay(500)).Unwrap()
                .ContinueWith(task =>
                {
                    PrintWithThread("After delay");
                });

            PrintWithThread("Exiting DoSmthAsync method.");
            return task;
        }

        private async Task DoSmthAsync_2()
        {
            PrintWithThread("Entered DoSmthAsync method.");

            PrintWithThread("First line");

            var task = Task.Delay(500);
            await task;

            PrintWithThread("After delay");

            PrintWithThread("Exiting DoSmthAsync method.");
        }

        private async Task<int> DoSmthAsync_3()
        {
            PrintWithThread("First line");

            await Task.Delay(500);

            PrintWithThread("Second line");
            return 3;
        }

        private async Task DoSmthAsync_4()
        {
            PrintWithThread("First line");
        }

        // Optimize this method.
        private async Task DoSmthAsync_5()
        {
            PrintWithThread("First line");

            await Task.Delay(500);
        }
    }
}
