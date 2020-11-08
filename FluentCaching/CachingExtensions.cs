
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
        public static CombinedCachingKeyBuilder<T> UseSelfAsKey<T>(this T targetObject) =>
            new CachingKeyBuilder<T>(targetObject).UseSelfAsKey();

        public static CombinedCachingKeyBuilder<T>
            UseAsKey<T, TValue>(this T targetObject, Func<T, TValue> valueGetter) =>
            new CachingKeyBuilder<T>(targetObject).UseAsKey(valueGetter);

        public static CombinedCachingKeyBuilder<T> UseAsKey<T, TValue>(this T targetObject, TValue value) =>
            new CachingKeyBuilder<T>(targetObject).UseAsKey(value);

        public static Task CacheAsync<T>(this T targetObject) =>
            StoringHelper.StoreAsync(targetObject);
    }
}
