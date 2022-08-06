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

        ICacheConfigurationItem GetItem<T>()
            where T : class;

        ICacheConfiguration For<T>(Func<CachingKeyPolicyBuilder<T>, AndPolicyBuilder<CacheImplementationPolicyBuilder>> factoryFunc)
            where T : class;

        ICacheConfiguration For<T>(Func<CachingKeyPolicyBuilder<T>, CacheImplementationPolicyBuilder> factoryFunc)
            where T : class;

        ICacheConfiguration SetGenericCache(ICacheImplementation cacheImplementation);
    }
}