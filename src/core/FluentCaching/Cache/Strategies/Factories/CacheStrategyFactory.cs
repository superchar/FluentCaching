using FluentCaching.Cache.Models;
using FluentCaching.Cache.Strategies.Remove;
using FluentCaching.Cache.Strategies.Retrieve;
using FluentCaching.Cache.Strategies.Store;
using FluentCaching.Configuration;

namespace FluentCaching.Cache.Strategies.Factories;

internal class CacheStrategyFactory : ICacheStrategyFactory
{
    private readonly ICacheConfiguration _cacheConfiguration;

    public CacheStrategyFactory(ICacheConfiguration cacheConfiguration)
    {
        _cacheConfiguration = cacheConfiguration;
    }

    public IStoreStrategy<TEntity> CreateStoreStrategy<TEntity>() where TEntity : class
        => new StoreStrategy<TEntity>(_cacheConfiguration);

    public IRetrieveStrategy<TEntity> CreateRetrieveStrategy<TEntity>(CacheSource<TEntity> source) where TEntity : class
        => source.CacheSourceType switch
        {
            CacheSourceType.Static => new StaticKeyRetrieveStrategy<TEntity>(_cacheConfiguration),
            CacheSourceType.Scalar => new ScalarKeyRetrieveStrategy<TEntity>(_cacheConfiguration),
            _ => new ComplexKeyRetrieveStrategy<TEntity>(_cacheConfiguration)
        };

    public IRemoveStrategy<TEntity> CreateRemoveStrategy<TEntity>(CacheSource<TEntity> source) where TEntity : class
        => source.CacheSourceType switch
        {
            CacheSourceType.Scalar => new ScalarKeyRemoveStrategy<TEntity>(_cacheConfiguration),
            CacheSourceType.Static => new StaticKeyRemoveStrategy<TEntity>(_cacheConfiguration),
            _ => new ComplexKeyRemoveStrategy<TEntity>(_cacheConfiguration)
        };
}