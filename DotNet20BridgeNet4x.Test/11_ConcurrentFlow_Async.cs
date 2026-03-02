using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace Concurrency
{
    public static class PerThreadData
    {
        [ThreadStatic]
        public static string MyData;
    }

    public class ConcurrentFlow_Async : Example
    {
        public override int Id => 11;

        public override async Task RunAsync()
        {
            CultureInfo.CurrentCulture = new CultureInfo("by");
            PerThreadData.MyData = "my data";

            //ExecutionContext.SuppressFlow();

            PrintWithThread("main method");
            PrintWithThread($"culture: {CultureInfo.CurrentCulture}");
            PrintWithThread($"data: {PerThreadData.MyData}");

            await Task.Delay(100);

            PrintWithThread($"culture: {CultureInfo.CurrentCulture}");
            PrintWithThread($"data: {PerThreadData.MyData}");
        }
    }
}
