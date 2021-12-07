using System;
using System.Threading.Tasks;
using FluentCaching.Cache.Helpers;
using FluentCaching.Configuration;
using FluentCaching.Exceptions;

namespace FluentCaching.Cache
{
    internal class StoringService : IStoringService
    {
        private readonly IConcurrencyHelper _concurrencyHelper;

        private readonly ICacheConfiguration _configuration;

        public StoringService(ICacheConfiguration configuration, IConcurrencyHelper concurrencyHelper)
        {
            _configuration = configuration;
            _concurrencyHelper = concurrencyHelper;
        }

        public Task StoreAsync<T>(T targetObject) where T : class
        {
            var item = GetConfigurationItem<T>();
            var key = item.Tracker.GetStoreKey(targetObject);
            return GetCacheImplementation(item)
                .SetAsync(key, targetObject, item.Options);
        }

        public Task<T> RetrieveAsync<T>(object targetObject) where T : class
        {
            var item = GetConfigurationItem<T>();
            var key = item.Tracker.GetRetrieveKeyComplex(targetObject);
            return GetCacheImplementation(item)
                .GetAsync<T>(key);
        }

        public Task<T> RetrieveAsync<T>(string targetString) where T : class
        {
            var item = GetConfigurationItem<T>();
            var key = item.Tracker.GetRetrieveKeySimple(targetString);
            return GetCacheImplementation(item)
                .GetAsync<T>(key);
        }

        public Task RemoveAsync<T>(object targetObject) where T : class
        {
            var item = GetConfigurationItem<T>();
            var key = item.Tracker.GetRetrieveKeyComplex(targetObject);
            return GetCacheImplementation(item).RemoveAsync(key);
        }

        public Task RemoveAsync<T>(string targetString) where T : class
        {
            var item = GetConfigurationItem<T>();
            var key = item.Tracker.GetRetrieveKeySimple(targetString);
            return GetCacheImplementation(item)
                .RemoveAsync(key);
        }

        public Task<T> RetrieveOrStoreAsync<T>(string key, Func<string, Task<T>> entityFetcher) where T : class
            => RetrieveOrStoreAsync((object)key, k => entityFetcher((string)k));

        public Task<T> RetrieveOrStoreAsync<T>(object key, Func<object, Task<T>> entityFetcher) where T : class
           => _concurrencyHelper
               .ExecuteAsync(key, async k =>
               {
                   var value = await RetrieveAsync<T>(k);

                   if (value is null)
                   {
                       value = await entityFetcher(k);
                       await StoreAsync(value);
                   }

                   return value;
               });

        private CacheConfigurationItem<T> GetConfigurationItem<T>() where T : class =>
            _configuration.GetItem<T>() ?? throw new ConfigurationNotFoundException(typeof(T));

        private ICacheImplementation GetCacheImplementation<T>(CacheConfigurationItem<T> item) where T : class =>
            item.Options.CacheImplementation ??
            _configuration.Current ??
            throw new ConfigurationNotFoundException(typeof(T));
    }
}
