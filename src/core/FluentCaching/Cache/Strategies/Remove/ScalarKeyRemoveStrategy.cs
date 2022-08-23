using System.Threading.Tasks;
using FluentCaching.Cache.Models;
using FluentCaching.Configuration;

namespace FluentCaching.Cache.Strategies.Remove;

internal class ScalarKeyRemoveStrategy<T> : BaseCacheStrategyWithConfiguration, IRemoveStrategy<T>
    where T : class
{
    public ScalarKeyRemoveStrategy(ICacheConfiguration configuration) : base(configuration)
    {
    }

    public Task RemoveAsync(CacheSource<T> source)
    {
        var item = GetConfigurationItem<T>();
        var key = item.Options.KeyBuilder.BuildFromScalarKey(source.Key);
        return GetCacheImplementation<T>(item)
            .RemoveAsync(key);    
    }
}