using System;
using System.Collections.Generic;
using FluentCaching.Cache;
using FluentCaching.Cache.Models;
using FluentCaching.Configuration.PolicyBuilders;
using FluentCaching.Configuration.PolicyBuilders.Keys;
using FluentCaching.Keys.Builders.Factories;

namespace FluentCaching.Configuration;

internal sealed class CacheConfiguration(IKeyBuilderFactory keyBuilderFactory) : ICacheConfiguration
{
    public const string DefaultPolicyName = "default";

    private readonly Dictionary<Type, Dictionary<string, ICacheConfigurationItem>> _predefinedConfigurations = new();

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
            factoryFunc(new CachingKeyPolicyBuilder<TEntity>(keyBuilderFactory.CreateKeyBuilder<TEntity>())).And()
                .CachingOptions);

    public ICacheConfiguration For<TEntity>(
        Func<CachingKeyPolicyBuilder<TEntity>, CacheImplementationPolicyBuilder> factoryFunc)
        where TEntity : class
        => For<TEntity>(
            factoryFunc(new CachingKeyPolicyBuilder<TEntity>(keyBuilderFactory.CreateKeyBuilder<TEntity>()))
                .CachingOptions);

    public ICacheConfigurationItem? GetItem<TEntity>(string policyName) where TEntity : class =>
        _predefinedConfigurations.TryGetValue(typeof(TEntity), out var typeConfiguration)
            ? typeConfiguration.GetValueOrDefault(policyName)
            : null;

    private CacheConfiguration For<TEntity>(CacheOptions options)
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