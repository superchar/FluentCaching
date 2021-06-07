namespace FluentCaching.Configuration
{
    public abstract class CachingConfigurationBase
    {
        internal abstract ICacheImplementation Current { get; }

        internal abstract CachingConfigurationItem<T> GetItem<T>()
            where T : class;
    }
}