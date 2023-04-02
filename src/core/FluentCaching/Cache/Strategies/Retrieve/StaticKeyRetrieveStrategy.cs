using System.Threading.Tasks;
using FluentCaching.Cache.Models;
using FluentCaching.Configuration;

namespace FluentCaching.Cache.Strategies.Retrieve;

internal class StaticKeyRetrieveStrategy<TEntity> : BaseCacheStrategyWithConfiguration, IRetrieveStrategy<TEntity>
    where TEntity : class
{
    public StaticKeyRetrieveStrategy(ICacheConfiguration configuration) : base(configuration)
    {
    }
    
    public ValueTask<TEntity> RetrieveAsync(CacheSource<TEntity> source)
    {
        var item = GetConfigurationItem<TEntity>();
        var key = item.Options.KeyBuilder.BuildFromStaticKey<TEntity>();
        return GetCacheImplementation<TEntity>(item)
            .RetrieveAsync<TEntity>(key);    
    }
}