using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DotNet20BridgeNet4x.Test
{
    class task_test14
    {
        //Task.FromResult的应用（示例）：
        static IDictionary<string, string> cache = new Dictionary<string, string>() {
                { "0001","A"},{ "0002","B"},
                { "0003","C"},{ "0004","D"},
                { "0005","E"},{ "0006","F" }
            };
        static Task<string> getValueFromCache(string key) {
            Console.WriteLine("getValueFromCache开始执行。。。"+Thread.CurrentThread.ManagedThreadId);
            string result = string.Empty;
            Thread.Sleep(TimeSpan.FromSeconds(5));
            Console.WriteLine("getValueFromCache结束执行。。。");
            if (cache.TryGetValue(key, out result)) {
                return Task.FromResult(result);
            }
            return Task.FromResult("");
            //return "";
        }
        public static void test() {
            Task<string> task =Task.Run<string>(()=> getValueFromCache("0006"));
            Console.WriteLine("主程序继续执行："+Thread.CurrentThread.ManagedThreadId);
            string result = task.Result;
            Console.WriteLine("result=" + result);
            Console.ReadLine();
        }
    }
}
