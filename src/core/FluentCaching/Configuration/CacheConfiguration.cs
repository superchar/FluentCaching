using System;
using System.Collections.Generic;
using FluentCaching.Cache;
using FluentCaching.Cache.Models;
using FluentCaching.Configuration.PolicyBuilders;
using FluentCaching.Configuration.PolicyBuilders.Keys;
using FluentCaching.Keys.Builders.Factories;

namespace FluentCaching.Configuration;

internal sealed class CacheConfiguration : ICacheConfiguration
{
    public const string DefaultPolicyName = "default";
    
    private readonly IKeyBuilderFactory _keyBuilderFactory;

    private readonly Dictionary<Type, Dictionary<string, ICacheConfigurationItem>> _predefinedConfigurations = new();

    public CacheConfiguration(IKeyBuilderFactory keyBuilderFactory)
    {
        _keyBuilderFactory = keyBuilderFactory;
    }

    public ICacheImplementation? Current { get; private set; }

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

    public ICacheConfigurationItem? GetItem<TEntity>(string policyName) where TEntity : class =>
        _predefinedConfigurations.TryGetValue(typeof(TEntity), out var typeConfiguration)
            ? typeConfiguration.TryGetValue(policyName, out var configurationItem)
                ? configurationItem
                : null
            : null;

    private ICacheConfiguration For<TEntity>(CacheOptions options)
        where TEntity : class
    {
        if (!_predefinedConfigurations.ContainsKey(typeof(TEntity)))
        {
            _predefinedConfigurations[typeof(TEntity)] = new Dictionary<string, ICacheConfigurationItem>();
        }

        _predefinedConfigurations[typeof(TEntity)][options.PolicyName] = new CacheConfigurationItem(options);
        return this;
    }
}