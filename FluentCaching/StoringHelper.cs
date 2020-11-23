
using System;
using System.Threading.Tasks;
using FluentCaching.Api;
using FluentCaching.Api.Key;
using FluentCaching.Configuration;
using FluentCaching.Parameters;

namespace FluentCaching
{
    internal static class StoringHelper
    {
        public static Task StoreAsync(CachingOptions cachingOptions)
        {
            var implementation = CachingConfiguration.Instance.Current;

            if (implementation == null)
            {
                throw new Exception("Cache implementation is not configured");
            }

            return implementation.SetAsync(cachingOptions);
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

            return factory(builder).CacheAsync();
        }

        public static Task<TResult> RetrieveAsync<TResult>(object key)
            where TResult : class
        {
            return Task.FromResult(default(TResult));
        }
    }
}
