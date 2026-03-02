using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DotNet20BridgeNet4x.Test
{
    class task_test6
    {
        //组合任务.ContinueWith
        public static void test() {
            //创建一个任务
            Task<int> task = new Task<int>(() => {
                int sum = 0;
                Console.WriteLine("使用task执行异步操作");
                for (int i = 0; i < 100; i++) {
                    sum += i;
                }
                return sum;
            });
            //启动任务，并安排到当前任务队列线程中执行任务(System.Threading.Tasks.TaskScheduler)
            task.Start();
            Console.WriteLine("主线程执行其他处理");
            //任务完成是执行处理
            Task cwt = task.ContinueWith(t => {
                Console.WriteLine("任务完成后的执行结果：" + t.Result);
            });
            task.Wait();
            cwt.Wait();
        }
    }
}
