using System;
using System.Collections.Generic;
using FluentCaching.Cache;
using FluentCaching.Cache.Models;
using FluentCaching.Configuration.PolicyBuilders;
using FluentCaching.Configuration.PolicyBuilders.Keys;
using FluentCaching.Keys.Builders;
using FluentCaching.Keys.Builders.Factories;

namespace FluentCaching.Configuration;

internal sealed class CacheConfiguration : ICacheConfiguration
{
    private readonly IKeyBuilderFactory _keyBuilderFactory;

    private readonly Dictionary<Type, ICacheConfigurationItem> _predefinedConfigurations = new();

    public CacheConfiguration(IKeyBuilderFactory keyBuilderFactory)
    {
        _keyBuilderFactory = keyBuilderFactory;
    }

    public ICacheImplementation Current { get; private set; }

    public ICacheConfiguration SetGenericCache(ICacheImplementation cacheImplementation)
    {
        Current = cacheImplementation
                  ?? throw new ArgumentNullException(nameof(cacheImplementation),
                      "Cache implementation cannot be null");
        return this;
    }

    public ICacheConfiguration For<TEntity>(
        Func<CachingKeyPolicyBuilder<TEntity>, AndPolicyBuilder<CacheImplementationPolicyBuilder>> factoryFunc)
        where TEntity : class
        => For<TEntity>(
            factoryFunc(new CachingKeyPolicyBuilder<TEntity>(_keyBuilderFactory.CreateKeyBuilder<TEntity>())).And()
                .CachingOptions);

    public ICacheConfiguration For<TEntity>(
        Func<CachingKeyPolicyBuilder<TEntity>, CacheImplementationPolicyBuilder> factoryFunc)
        where TEntity : class
        => For<TEntity>(
            factoryFunc(new CachingKeyPolicyBuilder<TEntity>(_keyBuilderFactory.CreateKeyBuilder<TEntity>()))
                .CachingOptions);

    public ICacheConfigurationItem GetItem<TEntity>() where TEntity : class =>
        _predefinedConfigurations.TryGetValue(typeof(TEntity), out var configurationItem)
            ? configurationItem
            : null;

    private ICacheConfiguration For<TEntity>(CacheOptions options)
        where TEntity : class
    {
        _predefinedConfigurations[typeof(TEntity)] = new CacheConfigurationItem(options);
        return this;
    }
}