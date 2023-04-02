using System;
using FluentCaching.Configuration.PolicyBuilders;
using FluentCaching.Configuration.PolicyBuilders.Keys;

namespace FluentCaching.Cache.Builders;

public interface ICacheBuilder
{
    ICache Build();

    // ReSharper disable once UnusedMemberInSuper.Global
    // ReSharper disable once UnusedMethodReturnValue.Global
    ICacheBuilder For<TEntity>(Func<CachingKeyPolicyBuilder<TEntity>, CacheImplementationPolicyBuilder> factoryFunc) where TEntity : class;

    ICacheBuilder For<TEntity>(Func<CachingKeyPolicyBuilder<TEntity>, AndPolicyBuilder<CacheImplementationPolicyBuilder>> factoryFunc) where TEntity : class;

    ICacheBuilder SetGenericCache(ICacheImplementation cacheImplementation);
}