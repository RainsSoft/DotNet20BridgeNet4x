using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DotNet20BridgeNet4x.Test
{
    class task_test12
    {
        //多个任务（实例）
        static int taskMethod(string name, int seconds) {
            Console.WriteLine("task {0} is running on a thread id {1}, is thread pool thread:{2}",
                name, Thread.CurrentThread.ManagedThreadId, Thread.CurrentThread.IsThreadPoolThread);
            Thread.Sleep(TimeSpan.FromSeconds(seconds));
            throw new Exception("task boom:" + name);
            return 42 * seconds;
        }
        public static void test() {
            try {
                var t1 = new Task<int>(() => taskMethod("task 3", 3));
                var t2 = new Task<int>(() => taskMethod("task 4", 2));
                var complexTask = Task.WhenAll(t1, t2);
                var exceptionHandler = complexTask.ContinueWith(t => {
                    //不会执行
                    Console.WriteLine("result: " + t.Result);
                }, TaskContinuationOptions.OnlyOnFaulted);
                t1.Start();
                t2.Start();
                Task.WaitAll(t1, t2);
                //

            }
            catch (AggregateException ex) {
                ex.Handle(e => {
                    Console.WriteLine(ex.Message);
                    return true;
                });
            }
            Console.ReadLine();
        }
    }
}
