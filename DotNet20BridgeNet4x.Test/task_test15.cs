using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DotNet20BridgeNet4x.Test
{
    class task_test15
    {
        //使用IProgress实现异步编程的进程通知，IProgress只提供了一个方法void Report(T value)，
        //通过Report方法把一个T类型的值报告给IProgress,然后IProgress的实现类Progress的构造函数接收类型为Action的形参，
        //通过这个委托让进度显示在UI界面中（示例）：
        interface IProgress<T>
        {
            int progress { get; }

            void Report(int i);
        }
        class Progress<T> : IProgress<T>
        {
            public int progress { get; set; }

            public void Report(int i) {
                if (OnProgress != null) {
                    OnProgress(i);
                }
            }
            public Action<int> OnProgress;
        }
        static void doProcessing(IProgress<int> process) {
            for (int i = 0; i < 100; i++) {
                Thread.Sleep(100);
                if (process != null) {
                    process.Report(i);
                }
            }
        }
        static async Task display() {
            //当前线程
            var progress = new Progress<int>() {
                OnProgress = (percent) => {
                    Console.Clear();
                    Console.WriteLine("{0}%", percent);
                }
            };
            //线程池线程
            await Task.Run(() => doProcessing(progress));
            Console.WriteLine("");
            Console.WriteLine("结束");
            Console.ReadLine();
        }
        public static void test() {
            Task task = display();
            task.Wait();
        }
    }
}
