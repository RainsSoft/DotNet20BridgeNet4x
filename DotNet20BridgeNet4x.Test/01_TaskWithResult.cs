using System.Threading.Tasks;

namespace Concurrency
{
    public class TaskWithResult : Example
    {
        public override int Id => 1;

        public override Task RunAsync()
        {
            PrintWithThread("main method");

            var taskWithoutResult = Task.Run(() =>
            {
                PrintWithThread("Inside task 1");
            });

            /*var task = new Task<int>(() => 3);
            task.Start();*/

            var taskWithResult = Task.Run<int>(() =>
            {
                PrintWithThread("Inside task 2");
                return 3;
            });

            //taskWithResult.Wait();
            var result = taskWithResult.Result;
            PrintWithThread($"Result = {result}");

            return Task.CompletedTask;
        }
    }
}
