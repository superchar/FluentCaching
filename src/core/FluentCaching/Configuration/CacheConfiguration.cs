using System;
using System.Collections.Generic;
using FluentCaching.Cache;
using FluentCaching.Cache.Models;
using FluentCaching.Configuration.PolicyBuilders;
using FluentCaching.Configuration.PolicyBuilders.Keys;

namespace FluentCaching.Configuration;

internal sealed class CacheConfiguration : ICacheConfiguration
{
    private readonly Dictionary<Type, ICacheConfigurationItem> _predefinedConfigurations = new ();

    public ICacheImplementation Current { get; private set; }

    public ICacheConfiguration SetGenericCache(ICacheImplementation cacheImplementation)
    {
        Current = cacheImplementation 
                               ?? throw new ArgumentNullException(nameof(cacheImplementation), "Cache implementation cannot be null");
        return this;
    }

    public ICacheConfiguration For<T>(Func<CachingKeyPolicyBuilder<T>, AndPolicyBuilder<CacheImplementationPolicyBuilder>> factoryFunc)
        where T : class
        => For<T>(factoryFunc(new CachingKeyPolicyBuilder<T>()).And().CachingOptions);

    public ICacheConfiguration For<T>(Func<CachingKeyPolicyBuilder<T>, CacheImplementationPolicyBuilder> factoryFunc)
        where T : class
        => For<T>(factoryFunc(new CachingKeyPolicyBuilder<T>()).CachingOptions);

    public ICacheConfigurationItem GetItem<T>() where T : class =>
        _predefinedConfigurations.TryGetValue(typeof(T), out var configurationItem)
            ? configurationItem
            : null;

    private ICacheConfiguration For<T>(CacheOptions options)
        where T : class
    {
        _predefinedConfigurations[typeof(T)] = new CacheConfigurationItem(options);
        return this;
    }
}