using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DotNet20BridgeNet4x.Test
{
    class task_test3
    {
        async static void AsyncFunction() {
            await Task.Delay(1000 * 5);
            Console.WriteLine("使用System.Threading.Tasks.Task执行异步操作");
            for (int i = 0; i < 10; i++) {
                Console.WriteLine(string.Format("AsyncFunction:i={0}", i));
            }
            await Task.Delay(1000 * 5);
            Console.WriteLine("使用System.Threading.Tasks.Task执行异步操作2");
            for (int i = 0; i < 10; i++) {
                Console.WriteLine(string.Format("AsyncFunction:i={0}", i));
            }
        }
        internal static void test() {
            Console.WriteLine("主线程执行业务处理");
            AsyncFunction();//异步执行
            Console.WriteLine("主线程执行其他处理");
            Task.WaitAll();//对异步方法没有阻塞效果
            Console.ReadLine();
        }
    }
}
