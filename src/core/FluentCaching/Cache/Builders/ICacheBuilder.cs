using System;
using FluentCaching.Configuration.PolicyBuilders;
using FluentCaching.Configuration.PolicyBuilders.Keys;

namespace FluentCaching.Cache.Builders;

public interface ICacheBuilder
{
    ICache Build();

    // ReSharper disable once UnusedMemberInSuper.Global
    // ReSharper disable once UnusedMethodReturnValue.Global
    ICacheBuilder For<T>(Func<CachingKeyPolicyBuilder<T>, CacheImplementationPolicyBuilder> factoryFunc) where T : class;

    ICacheBuilder For<T>(Func<CachingKeyPolicyBuilder<T>, AndPolicyBuilder<CacheImplementationPolicyBuilder>> factoryFunc) where T : class;

    ICacheBuilder SetGenericCache(ICacheImplementation cacheImplementation);
}