using System;
using System.Threading.Tasks;
using FluentCaching.Configuration;
using FluentCaching.Exceptions;

namespace FluentCaching
{
    internal class StoringService<T>
        where T : class
    {
        private readonly CachingConfigurationBase _configuration;

        public StoringService(CachingConfigurationBase configuration)
        {
            _configuration = configuration;
        }

        public Task StoreAsync(T targetObject)
        {
            var item = GetConfigurationItem();
            var key = item.Tracker.GetStoreKey(targetObject);
            return GetCacheImplementation(item)
                .SetAsync(key, targetObject, item.Options);
        }

        public async Task<T> StoreAsync(Task<T> retrieveFromCacheTask, Func<Task<T>> entityFetcher)
        {
            var result = await retrieveFromCacheTask;

            if (result != null)
            {
                return result;
            }

            result = await entityFetcher();

            if (result != null)
            {
                await StoreAsync(result);
            }

            return result;
        }

        public Task<T> RetrieveAsync(object targetObject)
        {
            var item = GetConfigurationItem();
            var key = item.Tracker.GetRetrieveKeyComplex(targetObject);
            return GetCacheImplementation(item)
                .GetAsync<T>(key);
        }

        public Task<T> RetrieveAsync(string targetString)
        {
            var item = GetConfigurationItem();
            var key = item.Tracker.GetRetrieveKeySimple(targetString);
            return GetCacheImplementation(item)
                .GetAsync<T>(key);
        }

        public Task RemoveAsync(object targetObject)
        {
            var item = GetConfigurationItem();
            var key = item.Tracker.GetRetrieveKeyComplex(targetObject);
            return GetCacheImplementation(item).RemoveAsync(key);
        }

        public Task RemoveAsync(string targetString)
        {
            var item = GetConfigurationItem();
            var key = item.Tracker.GetRetrieveKeySimple(targetString);
            return GetCacheImplementation(item)
                .RemoveAsync(key);
        }

        private CachingConfigurationItem<T> GetConfigurationItem() =>
            _configuration.GetItem<T>() ?? throw new ConfigurationNotFoundException(typeof(T));

        private ICacheImplementation GetCacheImplementation(CachingConfigurationItem<T> item) =>
            item.Options.CacheImplementation ??
            _configuration.Current ??
            throw new ConfigurationNotFoundException(typeof(T));
    }
}
