using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Concurrent;
using DotNet20BridgeNet4x.Test.EasyDeferred.Coroutine.Ellpeck;

namespace DotNet20BridgeNet4x.Test
{
    class Program
    {
        static void Main(string[] args) {
            //CoroutineExample.Test();
            //Console.ReadLine();
            task_test1.test();
            task_test2.test();
            task_test3.test();
            task_test4.test();
            task_test5.test();
            //
            task_test6.test();
            task_test7.test();
            task_test8.test();
            task_test9.test();
            task_test10.test();
            //
            task_test11.test();
            task_test12.test();
            task_test13.test();
            task_test14.test();
            task_test15.test();
            //
            task_test16.test();
            task_test17.test();        
            //test1
            {
                var t = Task.Run(async delegate {
                    //await Task.Delay(1000);
                    await Task.Delay(TimeSpan.FromSeconds(1.5));
                    return 42;
                });
                t.Wait();
                Console.WriteLine("Task t Status: {0}, Result: {1}",
                                  t.Status, t.Result);
                // The example displays the following output:
                //        Task t Status: RanToCompletion, Result: 42
            }
            //test2
            {
                Stopwatch sw = Stopwatch.StartNew();
                var delay = Task.Delay(1000).ContinueWith(_ => {
                    sw.Stop();
                    return sw.ElapsedMilliseconds;
                });

                Console.WriteLine("Elapsed milliseconds: {0}", delay.Result);
                // The example displays output like the following:
                //        Elapsed milliseconds: 1013
            }
            //test3
            {
                var delay2 = Task.Run(async () => {
                    Stopwatch sw2 = Stopwatch.StartNew();
                    await Task.Delay(2500);
                    sw2.Stop();
                    return sw2.ElapsedMilliseconds;
                });

                Console.WriteLine("Elapsed milliseconds: {0}", delay2.Result);
                // The example displays output like the following:
                //        Elapsed milliseconds: 2501
            }
            //test4
            {
                //It is not recommended to use in. NET2.0
                CancellationTokenSource source = new CancellationTokenSource();

                var t = Task.Run(async delegate {
                    await Task.Delay(1000 * 5, source.Token);
                    return 42;
                });
                source.Cancel();
                try {
                    t.Wait();
                }
                catch (AggregateException ae) {
                    foreach (var e in ae.InnerExceptions)
                        Console.WriteLine("{0}: {1}", e.GetType().Name, e.Message);
                }
                Console.Write("Task t Status: {0}", t.Status);
                if (t.Status == TaskStatus.RanToCompletion)
                    Console.Write(", Result: {0}", t.Result);
                source.Dispose();

                // The example displays the following output:
                //       TaskCanceledException: A task was canceled.
                //       Task t Status: Canceled
            }
            Console.ReadLine();
            //

            new test1().Add();
            Console.WriteLine("test1.add() out 2");
            Console.ReadLine();
            new test1().Add2();
            Console.WriteLine("test1.add2() out 5");
            Console.ReadLine();
            //输出2
            test2 t2 = new test2();
            t2.DoRun();
            //输出5
            Console.WriteLine("test2.DoRun() ");
            Console.ReadLine();

            //
            Console.WriteLine("-------主线程启动-------");
            Task<int> task = GetStrLengthAsync();
            Console.WriteLine("主线程继续执行");
            Console.WriteLine("Task返回的值" + task.Result);
            Console.WriteLine("-------主线程结束-------");
            Console.ReadLine();
        }


        static async Task<int> GetStrLengthAsync() {
            Console.WriteLine("GetStrLengthAsync方法开始执行");
            //此处返回的<string>中的字符串类型，而不是Task<string>
            string str = await GetString();
            Console.WriteLine("GetStrLengthAsync方法执行结束");
            return str.Length;
        }

        static Task<string> GetString() {
            //Console.WriteLine("GetString方法开始执行")
            return Task<string>.Run(() => {
                Thread.Sleep(2000);
                return "GetString的返回值";
            });
        }
        class test1
        {
            int count = 2;
            async void func1() {
                Console.WriteLine("t1[0]Task1 Start:========");
                await Task.Delay(1000);
                count += 3;
                Console.WriteLine("t1[2]Task1 End:==========");

            }
            public void Add() {
                Task.Factory.StartNew(func1).Wait();
                Console.WriteLine("t1[1]Console1 End, count = " + count);
                //输出2
            }

            int count2 = 2;
            async Task<int> func2() {
                Console.WriteLine("t2[0]Task2 Start:========");
                await Task.Delay(1000);
                count2 += 3;
                Console.WriteLine("t2[1]Task2 End:==========");
                return count2;
            }
            public void Add2() {
                Task.Factory.StartNew(func2).Result.Wait();
                Console.WriteLine("t1[2]Console2 End, count = " + count2);
                //输出5
            }
        }
        class test2
        {
            Stopwatch sw = new Stopwatch();
            public void DoRun() {
                Console.WriteLine(" before call:");
                ShowDelayAsync();
                Console.WriteLine(" after call:");
            }
            private async void ShowDelayAsync() {
                sw.Start();
                Console.WriteLine(" before delay:" + sw.ElapsedMilliseconds);
                await Task.Delay(1000 * 5);
                //System.Threading.Thread.Sleep(1000 * 5);
                Console.WriteLine(" after delay:" + sw.ElapsedMilliseconds);
            }
        }

    }

    internal static class CoroutineExample
    {

        private static readonly Event TestEvent = new Event();

        public static void Test() {
            var seconds = CoroutineHandler.Start(WaitSeconds(), "Awesome Waiting Coroutine");
            CoroutineHandler.Start(PrintEvery10Seconds(seconds));

            CoroutineHandler.Start(EmptyCoroutine());

            CoroutineHandler.InvokeLater(new Wait(5), () => {
                Console.WriteLine("Raising test event");
                CoroutineHandler.RaiseEvent(TestEvent);
            });
            CoroutineHandler.InvokeLater(new Wait(TestEvent), () => Console.WriteLine("Example event received"));

            CoroutineHandler.InvokeLater(new Wait(TestEvent), () => Console.WriteLine("I am invoked after 'Example event received'"), priority: -5);
            CoroutineHandler.InvokeLater(new Wait(TestEvent), () => Console.WriteLine("I am invoked before 'Example event received'"), priority: 2);

            var lastTime = DateTime.Now;
            while (true) {
                Console.WriteLine(CoroutineHandler.EventCount);
                var currTime = DateTime.Now;
                CoroutineHandler.Tick(currTime - lastTime);
                lastTime = currTime;
                Thread.Sleep(1);
            }
        }

        private static IEnumerator<Wait> WaitSeconds() {
            Console.WriteLine("First thing " + DateTime.Now);
            yield return new Wait(1);
            Console.WriteLine("After 1 second " + DateTime.Now);
            yield return new Wait(9);
            Console.WriteLine("After 10 seconds " + DateTime.Now);
            CoroutineHandler.Start(NestedCoroutine());
            yield return new Wait(5);
            Console.WriteLine("After 5 more seconds " + DateTime.Now);
            yield return new Wait(10);
            Console.WriteLine("After 10 more seconds " + DateTime.Now);

            yield return new Wait(20);
            Console.WriteLine("First coroutine done");
        }

        private static IEnumerator<Wait> PrintEvery10Seconds(ActiveCoroutine first) {
            while (true) {
                yield return new Wait(10);
                Console.WriteLine("The time is " + DateTime.Now);
                if (first.IsFinished) {
                    Console.WriteLine("By the way, the first coroutine has finished!");
                    Console.WriteLine($"{first.Name} data: {first.MoveNextCount} moves, " +
                                      $"{first.TotalMoveNextTime.TotalMilliseconds} total time, " +
                                      $"{first.LastMoveNextTime.TotalMilliseconds} last time");
                    Environment.Exit(0);
                }
            }
        }

        private static IEnumerator<Wait> EmptyCoroutine() {
            yield break;
        }

        private static IEnumerable<Wait> NestedCoroutine() {
            Console.WriteLine("I'm a coroutine that was started from another coroutine!");
            yield return new Wait(5);
            Console.WriteLine("It's been 5 seconds since a nested coroutine was started, yay!");
        }

    }
}
