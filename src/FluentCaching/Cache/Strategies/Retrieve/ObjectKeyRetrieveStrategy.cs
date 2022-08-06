using System.Threading.Tasks;
using FluentCaching.Cache.Models;
using FluentCaching.Configuration;

namespace FluentCaching.Cache.Strategies.Retrieve;

internal class ObjectKeyRetrieveStrategy<T> : BaseCacheStrategyWithConfiguration, IRetrieveStrategy<T>
    where T : class
{
    public ObjectKeyRetrieveStrategy(ICacheConfiguration configuration) : base(configuration)
    {
    }

    public Task<T> RetrieveAsync(CacheSource<T> source)
    {
        var item = GetConfigurationItem<T>();
        var key = item.Options.KeyBuilder.BuildFromObjectKey(source.ObjectKey);
        return GetCacheImplementation<T>(item)
            .RetrieveAsync<T>(key);
    }
}