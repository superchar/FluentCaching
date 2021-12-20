using FluentCaching.PolicyBuilders;
using FluentCaching.PolicyBuilders.Keys;
using FluentCaching.PolicyBuilders.Ttl;
using System;

namespace FluentCaching.Cache.Builders
{
    public interface ICacheBuilder
    {
        ICache Build();

        CacheBuilder For<T>(Func<CachingKeyBuilder<T>, CacheImplementationBuilder> factoryFunc) where T : class;
        
        CacheBuilder For<T>(Func<CachingKeyBuilder<T>, ExpirationTypeBuilder> factoryFunc) where T : class;
        
        CacheBuilder SetGenericCache(ICacheImplementation cacheImplementation);
    }
}