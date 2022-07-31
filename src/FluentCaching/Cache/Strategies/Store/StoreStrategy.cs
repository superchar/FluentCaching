using System.Threading.Tasks;
using FluentCaching.Configuration;

namespace FluentCaching.Cache.Strategies.Store;

internal class StoreStrategy<T> : BaseCacheStrategyWithConfiguration, IStoreStrategy<T>
    where T : class
{
    public StoreStrategy(ICacheConfiguration configuration) : base(configuration)
    {
    }

    public Task StoreAsync(T cachedObject)
    {
        var item = GetConfigurationItem<T>();
        var key = item.KeyBuilder.BuildFromCachedObject(cachedObject);
        return GetCacheImplementation(item)
            .CacheAsync(key, cachedObject, item.Options);
    }
}