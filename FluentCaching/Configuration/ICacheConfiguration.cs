using FluentCaching.Cache;

namespace FluentCaching.Configuration
{
    internal interface ICacheConfiguration
    {
        ICacheImplementation Current { get; }

        CacheConfigurationItem<T> GetItem<T>()
            where T : class;
    }
}