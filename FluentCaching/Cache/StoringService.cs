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

        public async Task<TValue> RetrieveOrStoreAsync<TKey, TValue>(TKey key, Func<TKey, Task<TValue>> entityFetcher)
           where TValue : class
        {
            var keyHash = _concurrencyHelper.TakeKeyLock(key);

            try
            {
                var value = await RetrieveAsync<TValue>(key.ToString());

                if (value is null)
                {
                    value = await entityFetcher(key);
                    await StoreAsync(value);
                }

                return value;
            }
            finally
            {
                _concurrencyHelper.ReleaseKeyLock(keyHash);
            }
        }

        public async Task<T> RetrieveOrStoreAsync<T>(object key, Func<object, Task<T>> entityFetcher) where T : class
        {
            var keyHash = _concurrencyHelper.TakeKeyLock(key);

            try
            {
                var value = await RetrieveAsync<T>(key);

                if (value is null)
                {
                    value = await entityFetcher(key);
                    await StoreAsync(value);
                }

                return value;
            }
            finally
            {
                _concurrencyHelper.ReleaseKeyLock(keyHash); 
            }
        }

        public async Task<TValue> RetrieveOrStoreAsync<TKey, TValue>(TKey key, Func<TKey, TValue> entityFetcher)
            where TValue : class
        {
            var keyHash = _concurrencyHelper.TakeKeyLock(key);

            try
            {
                var value = await RetrieveAsync<TValue>(key.ToString());

                if (value is null)
                {
                    value = entityFetcher(key);
                    await StoreAsync(value);
                }

                return value;
            }
            finally
            {
                _concurrencyHelper.ReleaseKeyLock(keyHash);
            }
        }

        public async Task<T> RetrieveOrStoreAsync<T>(object key, Func<object, T> entityFetcher) where T : class
        {
            var keyHash = _concurrencyHelper.TakeKeyLock(key);

            try
            {
                var value = await RetrieveAsync<T>(key);

                if (value is null)
                {
                    value = entityFetcher(key);
                    await StoreAsync(value);
                }

                return value;
            }
            finally
            {
                _concurrencyHelper.ReleaseKeyLock(keyHash);
            }
        }

        private CacheConfigurationItem<T> GetConfigurationItem<T>() where T : class =>
            _configuration.GetItem<T>() ?? throw new ConfigurationNotFoundException(typeof(T));

        private ICacheImplementation GetCacheImplementation<T>(CacheConfigurationItem<T> item) where T : class =>
            item.Options.CacheImplementation ??
            _configuration.Current ??
            throw new ConfigurationNotFoundException(typeof(T));
    }
}
