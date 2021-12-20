using System;
using System.Threading;

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
            var bucket = (uint)key.GetHashCode() % (uint)_keyLocks.Length;

            while (Interlocked.CompareExchange(ref _keyLocks[bucket], 1, 0) == 1)
            {
                Thread.Yield();
            }

            return bucket;
        }

        public void ReleaseKeyLock(uint keyBucket)
        {
            _keyLocks[keyBucket] = 0;
        }
    }
}
