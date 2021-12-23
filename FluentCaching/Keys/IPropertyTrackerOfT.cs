namespace FluentCaching.Keys
{
    internal interface IPropertyTracker<T> : IPropertyTracker where T : class
    {
        string GetStoreKey(T obj);
    }
}
