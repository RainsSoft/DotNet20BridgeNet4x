using System.Threading;
using System.Threading.Tasks;

namespace Concurrency
{
    public class UnblockingThreads : Example
    {
        public override int Id => 3;

        public override Task RunAsync()
        {
            PrintWithThread("main method");

            var task1 = Task.Run(() =>
            {
                PrintWithThread("Inside task 1");

                Thread.Sleep(500);
                PrintWithThread("Task 1 - waited 500 ms");

                for (var i = 0; i < 100; i++)
                {
                    DoSomethingUseful();
                }
                PrintWithThread("Task 1 - did something useful");

                return 10;
            });

            var task2 = Task.Run(() =>
            {
                PrintWithThread("Inside task 2");

                var waitingTask = Task.Delay(500);

                for (var i = 0; i < 100; i++)
                {
                    DoSomethingUseful();
                }
                PrintWithThread("Task 2 - did something useful");

                waitingTask.Wait();
                PrintWithThread("Task 2 - finished waiting for 500 ms");

                return 10;
            });

            Task.WaitAll(task1, task2);

            //var task = Task.WhenAll(task1, task2);
            //var task = Task.WhenAny(task1, task2);

            return Task.CompletedTask;
        }

        private void DoSomethingUseful()
        {
            Thread.Sleep(10);
        }
    }
}
