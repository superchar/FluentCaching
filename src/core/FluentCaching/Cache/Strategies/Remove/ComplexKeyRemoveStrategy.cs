using System.Threading.Tasks;
using FluentCaching.Cache.Models;
using FluentCaching.Configuration;

namespace FluentCaching.Cache.Strategies.Remove;

internal class ComplexKeyRemoveStrategy<T> : BaseCacheStrategyWithConfiguration, IRemoveStrategy<T>
    where T : class
{
    public ComplexKeyRemoveStrategy(ICacheConfiguration configuration) : base(configuration)
    {
    }

    public ValueTask RemoveAsync(CacheSource<T> source)
    {
        var item = GetConfigurationItem<T>();
        var key = item.Options.KeyBuilder.BuildFromComplexKey(source.Key);
        return GetCacheImplementation<T>(item)
            .RemoveAsync(key);
    }
}