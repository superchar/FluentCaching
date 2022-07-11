using System;

namespace FluentCaching.Cache.Helpers
{
    internal interface IConcurrencyHelper
    {
        uint TakeKeyLock<TKey>(TKey key);

        uint TakeKeyLock(Type type);

        void ReleaseKeyLock(uint keyHash);
    }
}