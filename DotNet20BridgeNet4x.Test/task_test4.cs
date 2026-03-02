using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DotNet20BridgeNet4x.Test
{
    class task_test4
    {
        //带返回值 task
        static Task<int> createTask(string name) {
            return new Task<int>(() => taskMethod(name));
        }

        static int taskMethod(string name) {
            Console.WriteLine(string.Format("task({0}) threadId({1}) threadPool({2})", name, Thread.CurrentThread.ManagedThreadId, Thread.CurrentThread.IsThreadPoolThread));
            Thread.Sleep(TimeSpan.FromSeconds(2));
            return 42;
        }
        static int getSum() {
            int sum = 0;
            Console.WriteLine("使用Task执行异步操作");
            for (int i = 0; i < 100; i++) {
                sum += i;
            }
            return sum;
        }
        public static void test() {
            taskMethod("main thread task");
            var task = createTask("task 1");
            task.Start();
            int result = task.Result;//会阻塞等待结果
            Console.WriteLine("task 1 result:" + result);
            //Task.WaitAll(task);
            Console.ReadLine();
            //
            task = createTask("task 2");
            //改任务会运行在主线程中
            task.RunSynchronously();
            result = task.Result;
            Console.WriteLine("task 2 result:" + result);
            //Task.WaitAll(task);
            Console.ReadLine();
            //
            task = createTask("task 3");
            Console.WriteLine("task 3 status:" + task.Status);
            task.Start();
            while (!task.IsCompleted) {
                Console.WriteLine(task.Status);
                Thread.Sleep(TimeSpan.FromSeconds(0.5));
            }
            Console.WriteLine("task 3 status:" + task.Status);
            result = task.Result;
            Console.WriteLine("task 3 result:" + result);
            Console.ReadLine();
            #region 常规使用方式
            //创建任务
            Task<int> getsumtask = new Task<int>(() => getSum());
            //启动任务，并安排到当前任务队列线程中执行任务(System.Threading.Tasks.TaskScheduler)
            getsumtask.Start();
            Console.WriteLine("主线程执行其他处理");
            //等待任务的完成执行过程
            getsumtask.Wait();
            //获得任务的执行结果
            Console.WriteLine("任务执行结果：" + getsumtask.Result);
            Console.ReadLine();
            //
            #endregion
        }
    }
}
