using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DotNet20BridgeNet4x.Test
{
    class task_test16
    {
        //Factory.FromAsync的应用(简APM模式(委托) 转换为任务)(BeginXXX和EndXXX)（示例）带回调方式的
        private delegate string AsynchronousTask(string threadName);
        private static string doTest(string threadName) {
            Console.WriteLine("starting...");
            Console.WriteLine("is thread pool:" + Thread.CurrentThread.IsThreadPoolThread);
            Thread.Sleep(TimeSpan.FromSeconds(2));
            Thread.CurrentThread.Name = threadName;
            return string.Format("thread name=" + Thread.CurrentThread.Name);
        }
        private static void callBack(IAsyncResult ar) {
            Console.WriteLine("start callback....");
            Console.WriteLine("state passed to a callback:" + ar.AsyncState);
            Console.WriteLine("is thread pool:" + Thread.CurrentThread.IsThreadPoolThread);
            Console.WriteLine("thread pool worker thread id:" + Thread.CurrentThread.ManagedThreadId);
        }
        public static void test() {
            //执行流程 先执行doTest()-callBack()-task.ContinueWith
            AsynchronousTask d = doTest;
            Console.WriteLine("option 1");
            Task<string> task = Task<string>.Factory.FromAsync(
                    d.BeginInvoke("asyncTaskThread", callBack, "a delegate asynchronous call")
                , d.EndInvoke);
            task.ContinueWith(t => {
                Console.WriteLine("callback is finished,now running a continue,result:" + t.Result);
            });
            while (!task.IsCompleted) {
                Console.WriteLine(task.Status);
                Thread.Sleep(TimeSpan.FromSeconds(0.5));
            }
            Console.WriteLine(task.Status);
            Console.ReadLine();
        }
    }
}

