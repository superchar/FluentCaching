using System.Threading.Tasks;
using FluentCaching.Cache.Models;
using FluentCaching.Configuration;

namespace FluentCaching.Cache.Strategies.Remove;

internal class StringKeyRemoveStrategy<T> : BaseCacheStrategyWithConfiguration, IRemoveStrategy<T>
    where T : class
{
    public StringKeyRemoveStrategy(ICacheConfiguration configuration) : base(configuration)
    {
    }

    public Task RemoveAsync(CacheSource<T> source)
    {
        var item = GetConfigurationItem<T>();
        var key = item.KeyBuilder.BuildFromStringKey(source.StringKey);
        return GetCacheImplementation(item)
            .RemoveAsync(key);    
    }
}