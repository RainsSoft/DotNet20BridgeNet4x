using System.Threading;
using System.Threading.Tasks;

namespace Concurrency
{
    public class Cache : Example
    {
        private readonly SemaphoreSlim _mutex;

        public override int Id => 16;
        public override async Task RunAsync()
        {
            PrintWithThread("Main method");

            var obj = new SemaphoreSlim(2, 2);

            Task.WaitAll(
                GetCachedValueAsync(obj),
                GetCachedValueAsync(obj),
                GetCachedValueAsync(obj),
                GetCachedValueAsync(obj),
                GetCachedValueAsync(obj));
        }

        private string _cachedValue;
        public async Task<string> GetCachedValueAsync(object obj)
        {
            if (_cachedValue != null)
                return _cachedValue;

            await _mutex.WaitAsync();
            try
            {
                await Task.Delay(100); // Do some work.

                _cachedValue = "value";
                return _cachedValue;
            }
            finally
            {
                _mutex.Release();
            }
        }
    }
}
