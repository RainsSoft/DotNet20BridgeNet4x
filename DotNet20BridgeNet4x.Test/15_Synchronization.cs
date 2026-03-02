using System.Threading;
using System.Threading.Tasks;

namespace Concurrency
{
    public class Synchronization : Example
    {
        public override int Id => 15;

        public override async Task RunAsync()
        {
            PrintWithThread("Main method");

            var obj = new SemaphoreSlim(2, 2);

            Task.WaitAll(
                DoSomeWorkAsync(obj),
                DoSomeWorkAsync(obj),
                DoSomeWorkAsync(obj),
                DoSomeWorkAsync(obj),
                DoSomeWorkAsync(obj));
        }

        public async Task DoSomeWorkAsync(object obj)
        {
            Monitor.Enter(obj);

            PrintWithThread("Doing some work.");
            Thread.Sleep(100);
            PrintWithThread("Done.");

            Monitor.Exit(obj);
        }
    }
}
