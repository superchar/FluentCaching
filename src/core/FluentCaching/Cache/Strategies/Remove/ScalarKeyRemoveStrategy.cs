using System.Threading.Tasks;
using FluentCaching.Cache.Models;
using FluentCaching.Configuration;

namespace FluentCaching.Cache.Strategies.Remove;

internal class ScalarKeyRemoveStrategy<TEntity> : BaseCacheStrategyWithConfiguration, IRemoveStrategy<TEntity>
    where TEntity : class
{
    public ScalarKeyRemoveStrategy(ICacheConfiguration configuration) : base(configuration)
    {
    }

    public ValueTask RemoveAsync(CacheSource<TEntity> source)
    {
        var item = GetConfigurationItem<TEntity>();
        var key = item.Options.KeyBuilder.BuildFromScalarKey(source.Key);
        return GetCacheImplementation<TEntity>(item)
            .RemoveAsync(key);    
    }
}