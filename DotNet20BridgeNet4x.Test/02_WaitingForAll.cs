using System.Threading.Tasks;

namespace Concurrency
{
    public class WaitingForAll : Example
    {
        public override int Id => 2;

        public override Task RunAsync()
        {
            PrintWithThread("main method");

            var task1 = Task.Run(() =>
            {
                PrintWithThread("Inside task 1");
                return 10;
            });

            var task2 = Task.Run(() =>
            {
                PrintWithThread("Inside task 2");
                return 20;
            });

            //var taskIndex = Task.WaitAny(task1, task2);
            Task.WaitAll(task1, task2);
            PrintWithThread($"Task 1 result = {task1.Result}");
            PrintWithThread($"Task 2 result = {task2.Result}");

            return Task.CompletedTask;
        }
    }
}
