using FluentCaching.Cache.Models;
using FluentCaching.Cache.Strategies.Remove;
using FluentCaching.Cache.Strategies.Retrieve;
using FluentCaching.Cache.Strategies.RetrieveOrStore;
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
        => source switch
        {
            { ObjectKey: null, StringKey: null } => new StaticKeyRetrieveStrategy<T>(_cacheConfiguration),
            { ObjectKey: null } => new StringKeyRetrieveStrategy<T>(_cacheConfiguration),
            _ => new ObjectKeyRetrieveStrategy<T>(_cacheConfiguration)
        };

    public IRemoveStrategy<T> CreateRemoveStrategy<T>(CacheSource<T> source) where T : class
        => source switch
        {
            { ObjectKey: null } => new StringKeyRemoveStrategy<T>(_cacheConfiguration),
            _ => new ObjectKeyRemoveStrategy<T>(_cacheConfiguration)
        };

    public IRetrieveOrStoreStrategy<T> CreateRetrieveOrStoreStrategy<T>(CacheSource<T> source) where T : class
    {
        var retrieveStrategy = CreateRetrieveStrategy(source);
        var cacheStrategy = CreateStoreStrategy<T>();

        return new CacheSourceRetrieveOrStoreStrategy<T>(cacheStrategy, retrieveStrategy);
    }
}