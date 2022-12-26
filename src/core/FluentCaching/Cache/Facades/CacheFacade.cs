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

        public ValueTask CacheAsync<T>(T targetObject) where T : class
            => _cacheStrategyFactory
                .CreateStoreStrategy<T>()
                .StoreAsync(targetObject);

        public ValueTask<T> RetrieveComplexAsync<T>(object complexKey) where T : class
            => RetrieveAsync(CacheSource<T>.CreateComplex(complexKey));

        public ValueTask<T> RetrieveScalarAsync<T>(object scalarKey) where T : class
            => RetrieveAsync(CacheSource<T>.CreateScalar(scalarKey));

        public ValueTask<T> RetrieveStaticAsync<T>() where T : class
            => RetrieveAsync(CacheSource<T>.Static);

        public ValueTask RemoveComplexAsync<T>(object complexKey) where T : class
            => RemoveAsync(CacheSource<T>.CreateComplex(complexKey));

        public ValueTask RemoveScalarAsync<T>(object scalarKey) where T : class
            => RemoveAsync(CacheSource<T>.CreateScalar(scalarKey));

        public ValueTask RemoveStaticAsync<T>() where T : class
            => RemoveAsync(CacheSource<T>.Static);
        
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
