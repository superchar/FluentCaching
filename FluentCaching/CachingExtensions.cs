
using System;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using FluentCaching.Api;
using FluentCaching.Api.Key;
using FluentCaching.Configuration;

namespace FluentCaching
{
    public static class CachingExtensions
    {
        public static CombinedCachingKeyBuilder<T> UseSelfAsKey<T>(this T targetObject) where T : class 
            => new CachingKeyBuilder<T>(targetObject).UseSelfAsKey();

        public static CombinedCachingKeyBuilder<T>  UseAsKey<T, TValue>(this T targetObject, Func<T, TValue> valueGetter) where T : class 
            => new CachingKeyBuilder<T>(targetObject).UseAsKey(valueGetter);

        public static CombinedCachingKeyBuilder<T> UseAsKey<T, TValue>(this T targetObject, TValue value) where T : class
            => new CachingKeyBuilder<T>(targetObject).UseAsKey(value);

        public static Task CacheAsync<T>(this T targetObject) where T : class
            => StoringHelper.StoreAsync(targetObject);

        public static Task<TEntity> RetrieveAsync<TEntity>(this object key) where TEntity : class
            => StoringHelper.RetrieveAsync<TEntity>(key);
    }
}
