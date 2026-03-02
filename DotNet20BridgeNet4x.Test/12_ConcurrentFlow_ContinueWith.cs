using System.Globalization;
using System.Threading.Tasks;

namespace Concurrency
{
    public class ConcurrentFlow_ContinueWith : Example
    {
        public override int Id => 12;

        public override async Task RunAsync()
        {
            CultureInfo.CurrentCulture = new CultureInfo("by");
            PerThreadData.MyData = "my data";

            PrintWithThread("main method");
            PrintWithThread($"culture: {CultureInfo.CurrentCulture}");
            PrintWithThread($"data: {PerThreadData.MyData}");

            await Task.Run(() =>
            {
                PrintWithThread($"1 - culture: {CultureInfo.CurrentCulture}");
                PrintWithThread($"1 - data: {PerThreadData.MyData}");

                CultureInfo.CurrentCulture = new CultureInfo("gb");
                PrintWithThread($"Updated culture: {CultureInfo.CurrentCulture}");

                PerThreadData.MyData = "another data";
            }).ContinueWith(task =>
            {
                PrintWithThread($"2 - culture: {CultureInfo.CurrentCulture}");
                PrintWithThread($"2 - data: {PerThreadData.MyData}");
            });
        }
    }
}
