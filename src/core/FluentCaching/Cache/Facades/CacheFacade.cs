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

        public Task<T> RetrieveComplexAsync<T>(object complexKey) where T : class
            => RetrieveAsync(CacheSource<T>.CreateComplex(complexKey));

        public Task<T> RetrieveScalarAsync<T>(object scalarKey) where T : class
            => RetrieveAsync(CacheSource<T>.CreateScalar(scalarKey));

        public Task<T> RetrieveStaticAsync<T>() where T : class
            => RetrieveAsync(CacheSource<T>.Static);

        public Task RemoveComplexAsync<T>(object complexKey) where T : class
            => RemoveAsync(CacheSource<T>.CreateComplex(complexKey));

        public Task RemoveScalarAsync<T>(object scalarKey) where T : class
            => RemoveAsync(CacheSource<T>.CreateScalar(scalarKey));

        public Task RemoveStaticAsync<T>() where T : class
            => RemoveAsync(CacheSource<T>.Static);
        
        private Task<T> RetrieveAsync<T>(CacheSource<T> source) where T : class
            => _cacheStrategyFactory
                .CreateRetrieveStrategy(source)
                .RetrieveAsync(source);
        
        private Task RemoveAsync<T>(CacheSource<T> source) where T : class
            => _cacheStrategyFactory
                .CreateRemoveStrategy(source)
                .RemoveAsync(source);
    }
}
