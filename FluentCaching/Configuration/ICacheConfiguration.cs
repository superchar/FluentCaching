using FluentCaching.Cache;
using FluentCaching.PolicyBuilders;
using FluentCaching.PolicyBuilders.Keys;
using FluentCaching.PolicyBuilders.Ttl;
using System;

namespace FluentCaching.Configuration
{
    internal interface ICacheConfiguration
    {
        ICacheImplementation Current { get; }

        ICacheConfigurationItem<T> GetItem<T>()
            where T : class;

        ICacheConfiguration For<T>(Func<CachingKeyBuilder<T>, AndBuilder<CacheImplementationBuilder>> factoryFunc)
            where T : class;

        ICacheConfiguration For<T>(Func<CachingKeyBuilder<T>, CacheImplementationBuilder> factoryFunc)
            where T : class;

        ICacheConfiguration SetGenericCache(ICacheImplementation cacheImplementation);
    }
}