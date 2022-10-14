using System;
using FluentCaching.Configuration.PolicyBuilders;
using FluentCaching.Configuration.PolicyBuilders.Keys;

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