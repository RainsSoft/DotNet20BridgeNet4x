using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DotNet20BridgeNet4x.Test
{
    class task_test13
    {
        //async/await的方式（示例）：
        static async Task throwNotImplementedExceptionAsync() {
            throw new NotImplementedException();
        }
        static async Task throwInvalidOperationAsync() {
            throw new InvalidOperationException();
        }
        static async Task Normal() {
            await Fun();
        }
        static Task Fun() {
            return Task.Run(() => {
                for (int i = 0; i < 10; i++) {
                    Console.WriteLine("i=" + i);
                    Thread.Sleep(200);
                }
            });
        }
        static async Task ObserverOneExceptionAsync() {
            var task1 = throwNotImplementedExceptionAsync();
            var task2 = throwInvalidOperationAsync();
            var task3 = Normal();
            try {
                //异步的方式
                Task allTask = Task.WhenAll(task1, task2, task3);
                await allTask;
                //同步方式
                //Task.WaitAll(task1,task2,task3);
            }
            catch (NotImplementedException ex) {
                Console.WriteLine("task1 任务报错");
            }
            catch (InvalidOperationException ex) {
                Console.WriteLine("task2 任务报错");
            }
            catch (Exception ex) {
                Console.WriteLine("任务报错");
            }
        }
        public static void test() {
            Task task = ObserverOneExceptionAsync();
            Console.WriteLine("主线程继续运行1");
            task.Wait();
            Console.WriteLine("主线程继续运行2");
        }
    }
}
