using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DotNet20BridgeNet4x.Test
{
    class task_test17
    {
        //Factory.FromAsync的应用 (简APM模式(委托)转换为任务)(BeginXXX和EndXXX)（示例）不带回调方式的：
        //Task启动带参数和返回值的函数任务
        //下面的例子test2是个带参数和返回值的函数
        private delegate int AsynchronousTask(string threadName);
        static AsynchronousTask callHandler;
        static int test2(object i) {
            callHandler = doInvokeMethod;
            callHandler.Invoke("aaa");
            System.Threading.Thread.Sleep(3000);
            Console.WriteLine("hello:" + i);
            callHandler.Invoke("bbb");
            return 0;
        }
        static int doInvokeMethod(string threadName) {
            //Thread.CurrentThread.Name = threadName;
            Thread.Sleep(TimeSpan.FromSeconds(1));
            int v = 0;
            for (int i = 0; i < 100; i++) {
                v += i;
            }
            Console.WriteLine(threadName + "  " + Thread.CurrentThread.ManagedThreadId + "    " + v);
            return v;
        }
        //测试调用
        public static void test() {
            object i = 55;
            var t = Task<int>.Factory.StartNew(new Func<object, int>(test2), i);
            t.Wait();
        }

    }
}
