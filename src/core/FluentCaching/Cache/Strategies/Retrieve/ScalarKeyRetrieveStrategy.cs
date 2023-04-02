using System.Threading.Tasks;
using FluentCaching.Cache.Models;
using FluentCaching.Configuration;

namespace FluentCaching.Cache.Strategies.Retrieve;

internal class ScalarKeyRetrieveStrategy<TEntity> : BaseCacheStrategyWithConfiguration, IRetrieveStrategy<TEntity>
    where TEntity : class
{
    public ScalarKeyRetrieveStrategy(ICacheConfiguration configuration) : base(configuration)
    {
    }
    
    public ValueTask<TEntity> RetrieveAsync(CacheSource<TEntity> source)
    {
        var item = GetConfigurationItem<TEntity>();
        var key = item.Options.KeyBuilder.BuildFromScalarKey(source.Key);
        return GetCacheImplementation<TEntity>(item)
            .RetrieveAsync<TEntity>(key);    
    }
}