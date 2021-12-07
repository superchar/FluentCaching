using System;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCaching.Cache.Helpers
{
    internal class ConcurrencyHelper : IConcurrencyHelper
    {
        private readonly int[] _keyLocks;

        public ConcurrencyHelper()
        {
            var lockCount = Math.Max(Environment.ProcessorCount * 8, 32);
            _keyLocks = new int[lockCount];
        }

        public Task<T> ExecuteAsync<T>(string key, Func<string, Task<T>> callback) where T : class
            => ExecuteAsync((object)key, k => callback((string)k));

        public async Task<T> ExecuteAsync<T>(object key, Func<object, Task<T>> callback) where T : class
        {
            var hash = (uint)key.GetHashCode() % (uint)_keyLocks.Length;

            while (Interlocked.CompareExchange(ref _keyLocks[hash], 1, 0) == 1)
            {
                Thread.Yield();
            }

            T result = null;

            try
            {
                result = await callback(key);
            }
            finally
            {
                _keyLocks[hash] = 0;
            }

            return result;
        }
    }
}
