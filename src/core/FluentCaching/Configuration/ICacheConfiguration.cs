using System;
using FluentCaching.Cache;
using FluentCaching.Configuration.PolicyBuilders;
using FluentCaching.Configuration.PolicyBuilders.Keys;

namespace FluentCaching.Configuration;

internal interface ICacheConfiguration
{
    ICacheImplementation Current { get; }

    ICacheConfigurationItem GetItem<TEntity>()
        where TEntity : class;

    ICacheConfiguration For<TEntity>(Func<CachingKeyPolicyBuilder<TEntity>, AndPolicyBuilder<CacheImplementationPolicyBuilder>> factoryFunc)
        where TEntity : class;

    ICacheConfiguration For<TEntity>(Func<CachingKeyPolicyBuilder<TEntity>, CacheImplementationPolicyBuilder> factoryFunc)
        where TEntity : class;

    ICacheConfiguration SetGenericCache(ICacheImplementation cacheImplementation);
}