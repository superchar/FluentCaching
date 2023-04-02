using System.Threading.Tasks;
using FluentCaching.Cache.Models;
using FluentCaching.Cache.Strategies.Factories;

namespace FluentCaching.Cache
{
    internal class Cache : ICache
    {
        private readonly ICacheStrategyFactory _cacheStrategyFactory;

        public Cache(ICacheStrategyFactory cacheStrategyFactory)
        {
            _cacheStrategyFactory = cacheStrategyFactory;
        }

        public ValueTask CacheAsync<TEntity>(TEntity targetObject) where TEntity : class
            => _cacheStrategyFactory
                .CreateStoreStrategy<TEntity>()
                .StoreAsync(targetObject);

        public ValueTask<TEntity> RetrieveAsync<TEntity>(object key) where TEntity : class
            => RetrieveAsync(CacheSource<TEntity>.Create(key));

        public ValueTask<TEntity> RetrieveAsync<TEntity>() where TEntity : class
            => RetrieveAsync(CacheSource<TEntity>.Create(null));

        public ValueTask RemoveAsync<TEntity>(object key) where TEntity : class
            => RemoveAsync(CacheSource<TEntity>.Create(key));

        public ValueTask RemoveAsync<TEntity>() where TEntity : class
            => RemoveAsync(CacheSource<TEntity>.Create(null));
        
        private ValueTask<TEntity> RetrieveAsync<TEntity>(CacheSource<TEntity> source) where TEntity : class
            => _cacheStrategyFactory
                .CreateRetrieveStrategy(source)
                .RetrieveAsync(source);
        
        private ValueTask RemoveAsync<TEntity>(CacheSource<TEntity> source) where TEntity : class
            => _cacheStrategyFactory
                .CreateRemoveStrategy(source)
                .RemoveAsync(source);
    }
}
