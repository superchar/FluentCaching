using FluentCaching.Cache.Models;
using FluentCaching.Cache.Strategies.Remove;
using FluentCaching.Cache.Strategies.Retrieve;
using FluentCaching.Cache.Strategies.Store;

namespace FluentCaching.Cache.Strategies.Factories;

internal interface ICacheStrategyFactory
{
    IStoreStrategy<TEntity> CreateStoreStrategy<TEntity>() where TEntity : class;

    IRetrieveStrategy<TEntity> CreateRetrieveStrategy<TEntity>(CacheSource<TEntity> source)  where TEntity : class;
    
    IRemoveStrategy<TEntity> CreateRemoveStrategy<TEntity>(CacheSource<TEntity> source)  where TEntity : class;
}