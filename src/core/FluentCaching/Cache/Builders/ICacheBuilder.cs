using System;
using FluentCaching.Configuration.PolicyBuilders;
using FluentCaching.Configuration.PolicyBuilders.Keys;
// ReSharper disable UnusedMemberInSuper.Global

namespace FluentCaching.Cache.Builders;

public interface ICacheBuilder
{
    ICache Build();

    // ReSharper disable once UnusedMethodReturnValue.Global
    ICacheBuilder For<TEntity>(Func<CachingKeyPolicyBuilder<TEntity>, CacheImplementationPolicyBuilder> factoryFunc) where TEntity : class;

    ICacheBuilder For<TEntity>(Func<CachingKeyPolicyBuilder<TEntity>, AndPolicyBuilder<CacheImplementationPolicyBuilder>> factoryFunc) where TEntity : class;

    // ReSharper disable once UnusedMember.Global
    ICacheBuilder SetGenericCache(ICacheImplementation cacheImplementation);
}