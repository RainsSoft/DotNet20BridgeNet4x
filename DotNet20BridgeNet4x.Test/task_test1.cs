using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DotNet20BridgeNet4x.Test
{
    class task_test1
    {
        public static void test() {
            Task t = new Task(() => {
                Console.WriteLine("任务开始工作");
                Thread.Sleep(1000 * 5);
            });
            t.Start();
            t.ContinueWith((task) => {
                Console.WriteLine("任务完成，完成时候的状态为：");
                Console.WriteLine(string.Format("IsCancel={0} \t IsCompleted={1} \t IsFaulted={2}", task.IsCanceled, task.IsCompleted, task.IsFaulted));
            });
            t.Wait();

        }
    }
}
