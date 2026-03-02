using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DotNet20BridgeNet4x.Test
{
    class task_test11
    {
        //处理任务中的异常（示例）：
        static int taskMethod(string name, int seconds) {
            Console.WriteLine(string.Format("task {0} is running on a thead id {1},is thread pool thread{2}",
                name, Thread.CurrentThread.ManagedThreadId, Thread.CurrentThread.IsThreadPoolThread));
            Thread.Sleep(TimeSpan.FromSeconds(seconds));
            throw new Exception("Boom!");
            return 42 * seconds;
        }
        public static void test() {
            try {
                Task<int> task = Task.Run(() => taskMethod("task 2", 2));
                int result = task.GetAwaiter().GetResult();
                Console.WriteLine("result:" + result);
            }
            catch (Exception ex) {
                Console.WriteLine("task 2 exception caught:" + ex.Message);

            }
            Console.WriteLine("------------------------");
            Console.WriteLine();
            Console.ReadLine();
        }
    }
}
