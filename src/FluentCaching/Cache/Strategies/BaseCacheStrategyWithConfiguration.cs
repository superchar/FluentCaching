using FluentCaching.Configuration;
using FluentCaching.Configuration.Exceptions;

namespace FluentCaching.Cache.Strategies;

internal abstract class BaseCacheStrategyWithConfiguration
{
    private readonly ICacheConfiguration _configuration;

    protected BaseCacheStrategyWithConfiguration(ICacheConfiguration configuration)
    {
        _configuration = configuration;
    }

    protected ICacheConfigurationItem GetConfigurationItem<T>() where T : class =>
        _configuration.GetItem<T>() ?? throw new ConfigurationNotFoundException(typeof(T));

    protected ICacheImplementation GetCacheImplementation<T>(ICacheConfigurationItem item) 
        where T : class =>
        item.Options.CacheImplementation ??
        _configuration.Current ??
        throw new CacheImplementationNotFoundException(typeof(T));
}