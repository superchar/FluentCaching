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

    protected ICacheConfigurationItem GetConfigurationItem<TEntity>() where TEntity : class =>
        _configuration.GetItem<TEntity>() ?? throw new ConfigurationNotFoundException(typeof(TEntity));

    protected ICacheImplementation GetCacheImplementation<TEntity>(ICacheConfigurationItem item) 
        where TEntity : class =>
        item.Options.CacheImplementation ??
        _configuration.Current ??
        throw new CacheImplementationNotFoundException(typeof(TEntity));
}