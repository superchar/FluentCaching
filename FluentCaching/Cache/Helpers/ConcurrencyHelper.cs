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

        public uint TakeKeyLock<TKey>(TKey key)
        {
            var hash = (uint)key.GetHashCode() % (uint)_keyLocks.Length;

            while (Interlocked.CompareExchange(ref _keyLocks[hash], 1, 0) == 1)
            {
                Thread.Yield();
            }

            return hash;
        }

        public void ReleaseKeyLock(uint keyHash)
        {
            _keyLocks[keyHash] = 0;
        }
    }
}
