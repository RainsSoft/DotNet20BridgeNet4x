using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace DotNet20BridgeNet4x.Test
{
    class task_test7
    {
        //任务的串行
        public static void test() {
            ConcurrentStack<int> stack = new ConcurrentStack<int>();
            //t1先串行
            var t1 = Task.Factory.StartNew(() => {
                stack.Push(1);
                stack.Push(2);
            });
            //t2,t3并行执行
            var t2 = t1.ContinueWith(t => {
                int result;
                stack.TryPop(out result);
                Console.WriteLine("task t2 result=" + result + " threadid=" + Thread.CurrentThread.ManagedThreadId);
            });
            //t2,t3并行执行
            var t3 = t1.ContinueWith(t => {
                int result;
                stack.TryPop(out result);
                Console.WriteLine("task t3 result=" + result + " threadid=" + Thread.CurrentThread.ManagedThreadId);
            });
            //等待t2与t3执行完成
            Task.WaitAll(t2, t3);

            //t4串行执行
            var t4 = Task.Factory.StartNew(() => {
                Console.WriteLine("当前元素个数：" + stack.Count + " threadid=" + Thread.CurrentThread.ManagedThreadId);
            });
            t4.Wait();
        }
    }
}
