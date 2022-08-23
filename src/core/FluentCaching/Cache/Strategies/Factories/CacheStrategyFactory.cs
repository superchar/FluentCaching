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

    public IStoreStrategy<T> CreateStoreStrategy<T>() where T : class
        => new StoreStrategy<T>(_cacheConfiguration);

    public IRetrieveStrategy<T> CreateRetrieveStrategy<T>(CacheSource<T> source) where T : class
        => source.CacheSourceType switch
        {
            CacheSourceType.Static => new StaticKeyRetrieveStrategy<T>(_cacheConfiguration),
            CacheSourceType.Scalar => new ScalarKeyRetrieveStrategy<T>(_cacheConfiguration),
            _ => new ComplexKeyRetrieveStrategy<T>(_cacheConfiguration)
        };

    public IRemoveStrategy<T> CreateRemoveStrategy<T>(CacheSource<T> source) where T : class
        => source.CacheSourceType switch
        {
            CacheSourceType.Scalar => new ScalarKeyRemoveStrategy<T>(_cacheConfiguration),
            CacheSourceType.Static => new StaticKeyRemoveStrategy<T>(_cacheConfiguration),
            _ => new ComplexKeyRemoveStrategy<T>(_cacheConfiguration)
        };
}