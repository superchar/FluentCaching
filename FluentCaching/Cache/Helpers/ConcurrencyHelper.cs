using System;
using System.Threading;

namespace FluentCaching.Cache.Helpers
{
    internal class ConcurrencyHelper : IConcurrencyHelper
    {
        private readonly int[] _keyLocks;

        public ConcurrencyHelper() : this(Math.Max(Environment.ProcessorCount * 8, 32))
        {
        }

        public ConcurrencyHelper(int locksCount)
        {
            _keyLocks = new int[locksCount];
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
            if(_keyLocks.Length <= keyBucket)
            {
                throw new ArgumentException("Key bucket is out of locks range", nameof(keyBucket));
            }

            _keyLocks[keyBucket] = 0;
        }
    }
}
