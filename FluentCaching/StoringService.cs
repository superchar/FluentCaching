
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentCaching.Api;
using FluentCaching.Api.Keys;
using FluentCaching.Configuration;
using FluentCaching.Exceptions;
using FluentCaching.Parameters;

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

        public Task StoreAsync(string key, T targetObject, CachingOptions cachingOptions) 
        {
            var implementation = _configuration.Current;

            if (implementation == null)
            {
                throw new ConfigurationNotFoundException(typeof(T));
            }

            return implementation.SetAsync(key, targetObject, cachingOptions);
        }

        public Task StoreAsync(T targetObject)
        {
            var item = GetConfigurationItem();

            var key = item.Tracker.GetStoreKey(targetObject);

            return StoreAsync(key, targetObject, item.Options);
        }

        public Task<T> RetrieveAsync(object targetObject)
        {
            var configurationItem = GetConfigurationItem();

            var key = configurationItem.Tracker.GetRetrieveKeyComplex(targetObject);

            return _configuration.Current.GetAsync<T>(key);
        }

        public Task<T> RetrieveAsync(string targetString)
        {
            var configurationItem = GetConfigurationItem();

            var key = configurationItem.Tracker.GetRetrieveKeySimple(targetString);

            return _configuration.Current.GetAsync<T>(key);
        }

        private CachingConfigurationItem<T> GetConfigurationItem()
        {
            var item = _configuration.GetItem<T>();

            if (item == null)
            {
                throw new ConfigurationNotFoundException(typeof(T));
            }

            return item;
        }
    }
}
