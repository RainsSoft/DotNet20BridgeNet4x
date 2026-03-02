using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DotNet20BridgeNet4x.Test
{
    class task_test10
    {
        //取消任务 CancellationTokenSource：
        static int taskMethod(string name, int seconds, CancellationToken token) {
            Console.WriteLine(string.Format("task {0} is running on a thread id {1}, is thread pool {2}",
                name, Thread.CurrentThread.ManagedThreadId, Thread.CurrentThread.IsThreadPoolThread));
            for (int i = 0; i < seconds; i++) {
                Thread.Sleep(TimeSpan.FromSeconds(1));
                if (token.IsCancellationRequested) {
                    return -1;
                }
            }
            return 42 * seconds;
        }
        public static void test() {
            var cts = new CancellationTokenSource();
            var longTask = new Task<int>(() => taskMethod("task 1", 10, cts.Token), cts.Token);
            Console.WriteLine(longTask.Status);
            cts.Cancel();
            Console.WriteLine(longTask.Status);
            Console.WriteLine("first task has been cancelled before execution");
            //
            cts = new CancellationTokenSource();
            longTask = new Task<int>(() => taskMethod("task 2", 10, cts.Token), cts.Token);
            longTask.Start();
            for (int i = 0; i < 5; i++) {
                Thread.Sleep(TimeSpan.FromSeconds(0.5));
                Console.WriteLine(longTask.Status);
            }
            cts.Cancel();
            for (int i = 0; i < 5; i++) {
                Thread.Sleep(TimeSpan.FromSeconds(0.5));
                Console.WriteLine(longTask.Status);
            }
            Console.WriteLine("a task has been completed with result:" + longTask.Result);
            //
            Console.ReadLine();
        }
    }
}
