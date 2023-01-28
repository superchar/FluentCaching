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

        public ValueTask CacheAsync<T>(T targetObject) where T : class
            => _cacheStrategyFactory
                .CreateStoreStrategy<T>()
                .StoreAsync(targetObject);

        public ValueTask<T> RetrieveAsync<T>(object key) where T : class
            => RetrieveAsync(CacheSource<T>.Create(key));

        public ValueTask<T> RetrieveAsync<T>() where T : class
            => RetrieveAsync(CacheSource<T>.Create(null));

        public ValueTask RemoveAsync<T>(object key) where T : class
            => RemoveAsync(CacheSource<T>.Create(key));

        public ValueTask RemoveAsync<T>() where T : class
            => RemoveAsync(CacheSource<T>.Create(null));
        
        private ValueTask<T> RetrieveAsync<T>(CacheSource<T> source) where T : class
            => _cacheStrategyFactory
                .CreateRetrieveStrategy(source)
                .RetrieveAsync(source);
        
        private ValueTask RemoveAsync<T>(CacheSource<T> source) where T : class
            => _cacheStrategyFactory
                .CreateRemoveStrategy(source)
                .RemoveAsync(source);
    }
}
