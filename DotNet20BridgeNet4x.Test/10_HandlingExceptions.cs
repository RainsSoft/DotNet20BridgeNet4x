using System;
using System.Threading.Tasks;

namespace Concurrency
{
    public class HandlingExceptions : Example
    {
        public override int Id => 10;

        public override async Task RunAsync()
        {
            PrintWithThread("main method");

            try
            {
                RunContinueWith().Wait();
            }
            catch (AggregateException exception)
            {
                PrintWithThread("aggregate exception");
            }

            try
            {
                RunWithAsync().Wait();
            }
            catch (AggregateException exception)
            {
                PrintWithThread("aggregate exception");
            }

            try
            {
                await RunContinueWith();
            }
            catch (InvalidOperationException)
            {
                PrintWithThread("invalid operation exception");
            }
        }

        public Task RunContinueWith()
        {
            var task = Task.Run(() =>
            {
                throw new InvalidOperationException();
            });

            task.ContinueWith(task =>
            {
                PrintWithThread("Success");
            }, TaskContinuationOptions.OnlyOnRanToCompletion);

            task.ContinueWith(task =>
            {
                PrintWithThread("Failed");

                var aggregateException = task.Exception;
                var exceptions = aggregateException.InnerExceptions;
            }, TaskContinuationOptions.OnlyOnFaulted);

            task.ContinueWith(task =>
            {
                PrintWithThread("Canceled");
            }, TaskContinuationOptions.OnlyOnFaulted);

            return task;
        }

        public async Task RunWithAsync()
        {
            var task = Task.Run(() =>
            {
                throw new InvalidOperationException();
            });

            await task;
        }
    }
}
