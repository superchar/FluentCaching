using FluentCaching.Cache.Models;
using FluentCaching.Cache.Strategies.Remove;
using FluentCaching.Cache.Strategies.Retrieve;
using FluentCaching.Cache.Strategies.RetrieveOrStore;
using FluentCaching.Cache.Strategies.Store;

namespace FluentCaching.Cache.Strategies.Factories;

internal interface ICacheStrategyFactory
{
    IStoreStrategy<T> CreateStoreStrategy<T>() where T : class;

    IRetrieveStrategy<T> CreateRetrieveStrategy<T>(CacheSource<T> source)  where T : class;
    
    IRemoveStrategy<T> CreateRemoveStrategy<T>(CacheSource<T> source)  where T : class;

    IRetrieveOrStoreStrategy<T> CreateRetrieveOrStoreStrategy<T>(CacheSource<T> source)  where T : class;

}