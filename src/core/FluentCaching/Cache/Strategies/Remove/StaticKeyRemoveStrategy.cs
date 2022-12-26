using System.Threading.Tasks;
using FluentCaching.Cache.Models;
using FluentCaching.Configuration;

namespace FluentCaching.Cache.Strategies.Remove;

internal class StaticKeyRemoveStrategy<T> : BaseCacheStrategyWithConfiguration,  IRemoveStrategy<T>
    where T : class
{
    public StaticKeyRemoveStrategy(ICacheConfiguration configuration) 
        : base(configuration)
    {
    }
    
    public ValueTask RemoveAsync(CacheSource<T> source)
    {
        var item = GetConfigurationItem<T>();
        var key = item.Options.KeyBuilder.BuildFromStaticKey();
        return GetCacheImplementation<T>(item)
            .RemoveAsync(key);        
    }
}