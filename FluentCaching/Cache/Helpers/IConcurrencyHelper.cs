namespace FluentCaching.Cache.Helpers
{
    internal interface IConcurrencyHelper
    {
        uint TakeKeyLock<TKey>(TKey key);

        void ReleaseKeyLock(uint keyHash);
    }
}