using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DotNet20BridgeNet4x.Test
{
    class task_test5
    {
        //async/await的实现方式（示例）
        async static Task<int> asyncGetSum() {
            await Task.Delay(1);
            Console.WriteLine("使用Task执行异步操作1");
            var task = Task.Run<float>(() => {
                float j = 0;
                for (int i = 1; i < short.MaxValue; i++) {
                    j += (float)Math.Sqrt((double)i);
                }
                return j;
            });
            await task;
            Console.WriteLine("result1:" + task.Result);
            int sum = 0;
            Console.WriteLine("使用Task执行异步操作2.");
            for (int i = 0; i < 100; i++) {
                sum += i;
            }
            return sum;
        }
        public static void test() {
            var ret1 = asyncGetSum();
            Console.WriteLine("主线程执行其他处理");
            for (int i = 0; i < 3; i++) {
                Console.WriteLine("Call Main()");
            }
            int result = ret1.Result;//阻塞主线程
            Console.WriteLine("任务执行结果：" + result);
        }
    }
}
