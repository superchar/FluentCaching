using System.Threading.Tasks;
using FluentCaching.Cache.Models;
using FluentCaching.Configuration;

namespace FluentCaching.Cache.Strategies.Retrieve;

internal class StaticKeyRetrieveStrategy<T> : BaseCacheStrategyWithConfiguration, IRetrieveStrategy<T>
    where T : class
{
    public StaticKeyRetrieveStrategy(ICacheConfiguration configuration) : base(configuration)
    {
    }
    
    public Task<T> RetrieveAsync(CacheSource<T> source)
    {
        var item = GetConfigurationItem<T>();
        var key = item.KeyBuilder.BuildFromStaticKey();
        return GetCacheImplementation(item)
            .RetrieveAsync<T>(key);    
    }
}