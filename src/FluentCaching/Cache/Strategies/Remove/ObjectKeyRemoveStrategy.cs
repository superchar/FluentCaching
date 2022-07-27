using System.Threading.Tasks;
using FluentCaching.Cache.Models;
using FluentCaching.Configuration;

namespace FluentCaching.Cache.Strategies.Remove;

internal class ObjectKeyRemoveStrategy<T> : BaseCacheStrategyWithConfiguration, IRemoveStrategy<T>
    where T : class
{
    public ObjectKeyRemoveStrategy(ICacheConfiguration configuration) : base(configuration)
    {
    }

    public Task RemoveAsync(CacheSource<T> source)
    {
        var item = GetConfigurationItem<T>();
        var key = item.KeyBuilder.BuildFromObjectKey(source.ObjectKey);
        return GetCacheImplementation(item)
            .RemoveAsync(key);
    }
}