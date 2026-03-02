using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Concurrency
{
    public class InterlockedExample : Example
    {
        private int _count = 1;

        public override int Id => 17;
        public override async Task RunAsync()
        {
            PrintWithThread("Main method");

            var tasks = Enumerable.Range(1, 10)
                .Select(index => Task.Run(() =>
                {
                    Thread.Sleep(100);

                    _count++;
                    PrintWithThread($"{_count}");

                    //var newValue = Interlocked.Increment(ref _count);
                    //PrintWithThread($"{newValue}");
                }))
                .ToArray();

            Task.WaitAll(tasks);
        }
    }
}
