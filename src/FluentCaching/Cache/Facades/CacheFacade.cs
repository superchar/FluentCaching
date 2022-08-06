using System;
using System.Threading.Tasks;
using FluentCaching.Cache.Models;
using FluentCaching.Cache.Strategies.Factories;

namespace FluentCaching.Cache.Facades
{
    internal class CacheFacade : ICacheFacade
    {
        private readonly ICacheStrategyFactory _cacheStrategyFactory;

        public CacheFacade(ICacheStrategyFactory cacheStrategyFactory)
        {
            _cacheStrategyFactory = cacheStrategyFactory;
        }

        public Task CacheAsync<T>(T targetObject) where T : class
            => _cacheStrategyFactory
                .CreateStoreStrategy<T>()
                .StoreAsync(targetObject);

        public Task<T> RetrieveAsync<T>(object targetObject) where T : class
            => RetrieveAsync(new CacheSource<T>(targetObject));

        public Task<T> RetrieveAsync<T>(string targetString) where T : class
            => RetrieveAsync(new CacheSource<T>(targetString));

        public Task<T> RetrieveAsync<T>() where T : class
            => RetrieveAsync(CacheSource<T>.Null);

        public Task RemoveAsync<T>(object objectKey) where T : class
            => RemoveAsync(new CacheSource<T>(objectKey));

        public Task RemoveAsync<T>(string stringKey) where T : class
            => RemoveAsync(new CacheSource<T>(stringKey));

        public Task<T> RetrieveOrStoreAsync<T>(object key, Func<object, Task<T>> entityFetcher) where T : class
            => RetrieveOrStoreAsync(new CacheSource<T>(key), _ => entityFetcher(_.ObjectKey));

        public Task<T> RetrieveOrStoreAsync<TKey, T>(TKey key, Func<string, Task<T>> entityFetcher) where T : class
            => RetrieveOrStoreAsync(new CacheSource<T>(key.ToString()), _ => entityFetcher(_.StringKey));

        public Task<T> RetrieveOrStoreAsync<T>(Func<Task<T>> entityFetcher) where T : class
            => RetrieveOrStoreAsync(CacheSource<T>.Null, (CacheSource<T> _) => entityFetcher());

        private Task<T> RetrieveAsync<T>(CacheSource<T> source) where T : class
            => _cacheStrategyFactory
                .CreateRetrieveStrategy(source)
                .RetrieveAsync(source);
        
        private Task RemoveAsync<T>(CacheSource<T> source) where T : class
            => _cacheStrategyFactory
                .CreateRemoveStrategy(source)
                .RemoveAsync(source);
        
        private Task<T> RetrieveOrStoreAsync<T>(CacheSource<T> source, 
            Func<CacheSource<T>, Task<T>> entityFetcher) where T : class
            => _cacheStrategyFactory
                .CreateRetrieveOrStoreStrategy(source)
                .RetrieveOrStoreAsync(source, entityFetcher);
    }
}
