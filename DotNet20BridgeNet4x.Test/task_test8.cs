using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DotNet20BridgeNet4x.Test
{
    class task_test8
    {
        //子任务
        public static void test() {
            Task<string[]> parent = new Task<string[]>((state) => {
                System.Threading.Thread.Sleep(1000);
                Console.WriteLine(state.ToString());
                string[] result = new string[2];
                //创建并启动子任务
                new Task(() => {
                    System.Threading.Thread.Sleep(1000);
                    result[0] = "我是子任务1";
                }, TaskCreationOptions.AttachedToParent).Start();
                new Task(() => {
                    System.Threading.Thread.Sleep(1000);
                    result[1] = "我是子任务2";
                }, TaskCreationOptions.AttachedToParent).Start();
                return result;
            },
            "我是父任务"
            );
            //任务处理完成后执行的操作
            parent.ContinueWith(t => {
                Thread.Sleep(1000);
                Array.ForEach(t.Result, r => Console.WriteLine(r));
            });
            //启动父任务
            parent.Start();
            //等待任务结束 wait只能等待父线程结束，没办法等到父线程的ContinueWith结束
            parent.Wait();
            Console.WriteLine("wait...");
            Console.ReadLine();
        }
    }
}
