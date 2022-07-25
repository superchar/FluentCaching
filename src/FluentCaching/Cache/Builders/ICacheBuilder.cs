using FluentCaching.PolicyBuilders;
using FluentCaching.PolicyBuilders.Keys;
using FluentCaching.PolicyBuilders.Ttl;
using System;

namespace FluentCaching.Cache.Builders
{
    public interface ICacheBuilder
    {
        ICache Build();

        ICacheBuilder For<T>(Func<CachingKeyPolicyBuilder<T>, CacheImplementationPolicyBuilder> factoryFunc) where T : class;

        ICacheBuilder For<T>(Func<CachingKeyPolicyBuilder<T>, AndPolicyBuilder<CacheImplementationPolicyBuilder>> factoryFunc) where T : class;

        ICacheBuilder SetGenericCache(ICacheImplementation cacheImplementation);
    }
}