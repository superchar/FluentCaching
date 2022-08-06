using System;
using System.Threading.Tasks;
using FluentCaching.Cache.Models;
using FluentCaching.Cache.Strategies.Retrieve;
using FluentCaching.Cache.Strategies.Store;

namespace FluentCaching.Cache.Strategies.RetrieveOrStore;

internal class CacheSourceRetrieveOrStoreStrategy<T> : IRetrieveOrStoreStrategy<T>
    where T : class
{
    private readonly IStoreStrategy<T> _storeStrategy;
    private readonly IRetrieveStrategy<T> _retrieveStrategy;
    
    public CacheSourceRetrieveOrStoreStrategy(
        IStoreStrategy<T> storeStrategy, 
        IRetrieveStrategy<T> retrieveStrategy)
    {
        _storeStrategy = storeStrategy;
        _retrieveStrategy = retrieveStrategy;
    }

    public async Task<T> RetrieveOrStoreAsync(CacheSource<T> source, Func<CacheSource<T>, Task<T>> entityFetcher)
    {
        var value = await _retrieveStrategy.RetrieveAsync(source);

        if (value is null)
        {
            value = await entityFetcher(source);
            await _storeStrategy.StoreAsync(value);
        }

        return value;
    }
}