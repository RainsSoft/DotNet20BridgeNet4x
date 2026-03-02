using System;
using System.Threading;
using System.Threading.Tasks;

namespace Concurrency
{
    public abstract class Example : IExample
    {
        public abstract int Id { get; }
        public abstract Task RunAsync();

        public static void PrintWithThread(string message)
        {
            Console.WriteLine($"{message} [{Thread.CurrentThread.ManagedThreadId}]");
        }
    }
}
