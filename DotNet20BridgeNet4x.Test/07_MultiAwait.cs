using System;
using System.Threading;
using System.Threading.Tasks;

namespace Concurrency
{
    public class MultiAwait : Example
    {
        public override int Id => 7;

        public override async Task RunAsync()
        {
            PrintWithThread("main method");

            var task = GetSmthAsync();

            await task;
            var result = await task;
            Console.WriteLine(await task);
            PrintWithThread($"{await task}");

            PrintWithThread("The end");
        }

        private async Task<int> GetSmthAsync()
        {
            await Task.Delay(100);

            //throw new InvalidOperationException();
            return 10;
        }
    }
}
