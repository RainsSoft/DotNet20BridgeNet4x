using System;
using System.Threading;
using System.Threading.Tasks;

namespace Concurrency
{
    // There's a bug here.
    public class ChainingTasks : Example
    {
        public override int Id => 4;

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

            var task3 = Task.Run(() =>
            {
                PrintWithThread("Inside task 3");
            });

            var task4 = task3.ContinueWith(task => Task.Delay(500));

            /*task4 = task3.ContinueWith(task =>
            {
                if (task.IsCompletedSuccessfully)
                    return Task.Delay(500);

                //return Task.CompletedTask;
                return Task.FromException(new InvalidOperationException());
            });*/

            /*task4 = task3.ContinueWith(
                task => Task.Delay(500),
                TaskContinuationOptions.OnlyOnRanToCompletion);
            task4 = task3.ContinueWith(
                task => Task.Run(() => Console.WriteLine("Failed!")),
                TaskContinuationOptions.OnlyOnFaulted);*/

            var task5 = task4.ContinueWith(task =>
            {
                for (var i = 0; i < 100; i++)
                {
                    DoSomethingUseful();
                }
                PrintWithThread("Task 3 - did something useful");

                return 10;
            });

            Console.WriteLine($"Result = {task5.Result}");

            return Task.CompletedTask;
        }

        private void DoSomethingUseful()
        {
            Thread.Sleep(10);
        }
    }
}
