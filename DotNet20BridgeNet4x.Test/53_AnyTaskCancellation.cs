using System;
using System.Threading;
using System.Threading.Tasks;

namespace Concurrency
{
    public class AnyTaskCancellation : Example
    {
        public override int Id => 53;
        public override async Task RunAsync()
        {
            PrintWithThread("Main method");

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));

            try
            {
                await SomeLibraryMethodAsync()
                    .WithCancellationAsync(cts.Token);
            }
            catch (OperationCanceledException)
            {
                PrintWithThread("Task is canceled");
            }

            PrintWithThread("The end");
        }

        public Task SomeLibraryMethodAsync()
        {
            return Task.Delay(TimeSpan.FromDays(1));
        }
    }

    public static class TaskExtensions
    {
        public static async Task WithCancellationAsync(this Task task, CancellationToken cancellationToken)
        {
            using var cts = new CancellationTokenSource();
            using var combinedCts = CancellationTokenSource.CreateLinkedTokenSource(
                cts.Token, cancellationToken);

            var delayTask = Task.Delay(Timeout.Infinite, combinedCts.Token);
            var firstCompleted = await Task.WhenAny(task, delayTask);

            if (firstCompleted == task)
                cts.Cancel();

            await firstCompleted;
        }
    }
}
