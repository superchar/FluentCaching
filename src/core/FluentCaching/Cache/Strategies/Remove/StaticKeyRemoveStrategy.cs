using System.Threading.Tasks;
using FluentCaching.Cache.Models;
using FluentCaching.Configuration;

namespace FluentCaching.Cache.Strategies.Remove;

internal class StaticKeyRemoveStrategy<TEntity> : BaseCacheStrategyWithConfiguration,  IRemoveStrategy<TEntity>
    where TEntity : class
{
    public StaticKeyRemoveStrategy(ICacheConfiguration configuration) 
        : base(configuration)
    {
    }
    
    public ValueTask RemoveAsync(CacheSource<TEntity> source, string policyName)
    {
        var item = GetConfigurationItem<TEntity>(policyName);
        var key = item.Options.KeyBuilder.BuildFromStaticKey<TEntity>();
        return GetCacheImplementation<TEntity>(item)
            .RemoveAsync(key);        
    }
}