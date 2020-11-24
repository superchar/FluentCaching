
using System;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using FluentCaching.Api;
using FluentCaching.Api.Keys;
using FluentCaching.Configuration;

namespace FluentCaching
{
    public static class CachingExtensions
    {
        public static Task CacheAsync<T>(this T targetObject) where T : class
            => StoringHelper.StoreAsync(targetObject);

        public static Task<TEntity> RetrieveAsync<TEntity>(this object key) where TEntity : class
            => StoringHelper.RetrieveAsync<TEntity>(key);
    }
}
