using System.Threading.Tasks;
using FluentCaching.Cache.Models;
using FluentCaching.Configuration;

namespace FluentCaching.Cache.Strategies.Retrieve;

internal class ComplexKeyRetrieveStrategy<T> : BaseCacheStrategyWithConfiguration, IRetrieveStrategy<T>
    where T : class
{
    public ComplexKeyRetrieveStrategy(ICacheConfiguration configuration) : base(configuration)
    {
    }

    public ValueTask<T> RetrieveAsync(CacheSource<T> source)
    {
        var item = GetConfigurationItem<T>();
        var key = item.Options.KeyBuilder.BuildFromComplexKey(source.Key);
        return GetCacheImplementation<T>(item)
            .RetrieveAsync<T>(key);
    }
}