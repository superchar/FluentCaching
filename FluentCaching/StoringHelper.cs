
using System;
using System.Threading.Tasks;
using FluentCaching.Api;
using FluentCaching.Api.Keys;
using FluentCaching.Configuration;
using FluentCaching.Parameters;

namespace FluentCaching
{
    internal static class StoringHelper
    {
        public static Task StoreAsync<T>(T targetObject, CachingOptions cachingOptions) where T : class
        {
            var implementation = CachingConfiguration.Instance.Current;

            if (implementation == null)
            {
                throw new Exception("Cache implementation is not configured");
            }

            return implementation.SetAsync(targetObject, cachingOptions);
        }

        public static Task StoreAsync<T>(T targetObject)
            where T : class
        {
            var factory = CachingConfiguration.Instance.GetFactory<T>();

            if (factory == null)
            {
                throw new Exception($"Missing configuration for type {typeof(T).FullName}");
            }

            var builder = new CachingKeyBuilder<T>(targetObject);

            var options = factory(builder).CachingOptions;

            return StoreAsync(targetObject, options);
        }

        public static Task<T> RetrieveAsync<T>(object targetObject)
            where T : class
        {
            var configurationItem = CachingConfiguration.Instance.GetItem<T>();

            var valueSource = configurationItem.Tracker.GetValueSourceDictionary(targetObject);

            var builder = new CachingKeyBuilder<T>(valueSource: valueSource);

            var key = configurationItem.Factory(builder).CachingOptions.Key;

            return CachingConfiguration.Instance.Current.GetAsync<T>(key);
        }
    }
}
