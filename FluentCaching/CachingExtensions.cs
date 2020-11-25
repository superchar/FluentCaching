
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

        public static Task<TEntity> RetrieveAsync<TEntity>(this int key) where TEntity : class
            => StoringHelper.RetrieveAsync<TEntity>(key.ToString());

        public static Task<TEntity> RetrieveAsync<TEntity>(this bool key) where TEntity : class
            => StoringHelper.RetrieveAsync<TEntity>(key.ToString());

        public static Task<TEntity> RetrieveAsync<TEntity>(this byte key) where TEntity : class
            => StoringHelper.RetrieveAsync<TEntity>(key.ToString());

        public static Task<TEntity> RetrieveAsync<TEntity>(this sbyte key) where TEntity : class
            => StoringHelper.RetrieveAsync<TEntity>(key.ToString());

        public static Task<TEntity> RetrieveAsync<TEntity>(this char key) where TEntity : class
            => StoringHelper.RetrieveAsync<TEntity>(key.ToString());

        public static Task<TEntity> RetrieveAsync<TEntity>(this decimal key) where TEntity : class
            => StoringHelper.RetrieveAsync<TEntity>(key.ToString());

        public static Task<TEntity> RetrieveAsync<TEntity>(this double key) where TEntity : class
            => StoringHelper.RetrieveAsync<TEntity>(key.ToString());

        public static Task<TEntity> RetrieveAsync<TEntity>(this float key) where TEntity : class
            => StoringHelper.RetrieveAsync<TEntity>(key.ToString());

        public static Task<TEntity> RetrieveAsync<TEntity>(this uint key) where TEntity : class
            => StoringHelper.RetrieveAsync<TEntity>(key.ToString());

        public static Task<TEntity> RetrieveAsync<TEntity>(this long key) where TEntity : class
            => StoringHelper.RetrieveAsync<TEntity>(key.ToString());

        public static Task<TEntity> RetrieveAsync<TEntity>(this ulong key) where TEntity : class
            => StoringHelper.RetrieveAsync<TEntity>(key.ToString());

        public static Task<TEntity> RetrieveAsync<TEntity>(this short key) where TEntity : class
            => StoringHelper.RetrieveAsync<TEntity>(key.ToString());

        public static Task<TEntity> RetrieveAsync<TEntity>(this ushort key) where TEntity : class
            => StoringHelper.RetrieveAsync<TEntity>(key.ToString());
    }
}
