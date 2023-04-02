using System.Threading.Tasks;
using FluentCaching.Configuration;

namespace FluentCaching.Cache.Strategies.Store;

internal class StoreStrategy<TEntity> : BaseCacheStrategyWithConfiguration, IStoreStrategy<TEntity>
    where TEntity : class
{
    public StoreStrategy(ICacheConfiguration configuration) : base(configuration)
    {
    }

    public ValueTask StoreAsync(TEntity cachedObject)
    {
        var item = GetConfigurationItem<TEntity>();
        var key = item.Options.KeyBuilder.BuildFromCachedObject(cachedObject);
        return GetCacheImplementation<TEntity>(item)
            .CacheAsync(key, cachedObject, item.Options);
    }
}