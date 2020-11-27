
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

        public Task StoreAsync(T targetObject, CachingOptions cachingOptions) 
        {
            var implementation = _configuration.Current;

            if (implementation == null)
            {
                throw new ConfigurationNotFoundException(typeof(T));
            }

            return implementation.SetAsync(targetObject, cachingOptions);
        }

        public Task StoreAsync(T targetObject)
        {
            var factory = GetConfigurationItem().Factory;

            var builder = new CachingKeyBuilder<T>(targetObject);

            var options = factory(builder).CachingOptions;

            return StoreAsync(targetObject, options);
        }

        public Task<T> RetrieveAsync(object targetObject)
        {
            var configurationItem = GetConfigurationItem();

            var valueSource = configurationItem.Tracker.GetValueSourceDictionary(targetObject);

            return RetrieveAsync(configurationItem, valueSource);
        }

        public Task<T> RetrieveAsync(string targetString)
        {
            var configurationItem = GetConfigurationItem();

            var valueSource = configurationItem.Tracker.GetValueSourceDictionary(targetString);

            return RetrieveAsync(configurationItem, valueSource);
        }

        private Task<T> RetrieveAsync(CachingConfigurationItem<T> configurationItem, IDictionary<string, object> valueSource)
        {
            var builder = new CachingKeyBuilder<T>(valueSource: valueSource);

            var key = configurationItem.Factory(builder).CachingOptions.Key;

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
