using System;
using System.Threading.Tasks;
using FluentCaching.Configuration;

namespace FluentCaching
{
    public static class CachingExtensions
    {
        #region CacheAsync

        public static Task CacheAsync<TEntity>(this TEntity targetObject) where TEntity : class
            => new StoringService<TEntity>(CachingConfiguration.Instance).StoreAsync(targetObject);

        public static Task<TEntity> CacheAsync<TEntity>(this Task<TEntity> retrieveFromCacheTask, Func<Task<TEntity>> entityFetcher) where TEntity : class
            => new StoringService<TEntity>(CachingConfiguration.Instance).StoreAsync(retrieveFromCacheTask, entityFetcher);

        internal static Task CacheAsync<TEntity>(this TEntity targetObject, CachingConfigurationBase configuration) where TEntity : class
            => new StoringService<TEntity>(configuration).StoreAsync(targetObject);

        internal static Task<TEntity> CacheAsync<TEntity>(this Task<TEntity> retrieveFromCacheTask, Func<Task<TEntity>> entityFetcher, CachingConfigurationBase configuration) where TEntity : class
            => new StoringService<TEntity>(configuration).StoreAsync(retrieveFromCacheTask, entityFetcher);

        #endregion

        #region RetrieveAsync
        public static Task<TEntity> RetrieveAsync<TEntity>(this object key) where TEntity : class
          => new StoringService<TEntity>(CachingConfiguration.Instance).RetrieveAsync(key);

        public static Task<TEntity> RetrieveAsync<TEntity>(this int key) where TEntity : class
            => new StoringService<TEntity>(CachingConfiguration.Instance).RetrieveAsync(key.ToString());

        public static Task<TEntity> RetrieveAsync<TEntity>(this bool key) where TEntity : class
            => new StoringService<TEntity>(CachingConfiguration.Instance).RetrieveAsync(key.ToString());

        public static Task<TEntity> RetrieveAsync<TEntity>(this byte key) where TEntity : class
            => new StoringService<TEntity>(CachingConfiguration.Instance).RetrieveAsync(key.ToString());

        public static Task<TEntity> RetrieveAsync<TEntity>(this sbyte key) where TEntity : class
            => new StoringService<TEntity>(CachingConfiguration.Instance).RetrieveAsync(key.ToString());

        public static Task<TEntity> RetrieveAsync<TEntity>(this char key) where TEntity : class
            => new StoringService<TEntity>(CachingConfiguration.Instance).RetrieveAsync(key.ToString());

        public static Task<TEntity> RetrieveAsync<TEntity>(this decimal key) where TEntity : class
            => new StoringService<TEntity>(CachingConfiguration.Instance).RetrieveAsync(key.ToString());

        public static Task<TEntity> RetrieveAsync<TEntity>(this double key) where TEntity : class
            => new StoringService<TEntity>(CachingConfiguration.Instance).RetrieveAsync(key.ToString());

        public static Task<TEntity> RetrieveAsync<TEntity>(this float key) where TEntity : class
            => new StoringService<TEntity>(CachingConfiguration.Instance).RetrieveAsync(key.ToString());

        public static Task<TEntity> RetrieveAsync<TEntity>(this uint key) where TEntity : class
            => new StoringService<TEntity>(CachingConfiguration.Instance).RetrieveAsync(key.ToString());

        public static Task<TEntity> RetrieveAsync<TEntity>(this long key) where TEntity : class
            => new StoringService<TEntity>(CachingConfiguration.Instance).RetrieveAsync(key.ToString());

        public static Task<TEntity> RetrieveAsync<TEntity>(this ulong key) where TEntity : class
            => new StoringService<TEntity>(CachingConfiguration.Instance).RetrieveAsync(key.ToString());

        public static Task<TEntity> RetrieveAsync<TEntity>(this short key) where TEntity : class
            => new StoringService<TEntity>(CachingConfiguration.Instance).RetrieveAsync(key.ToString());

        public static Task<TEntity> RetrieveAsync<TEntity>(this ushort key) where TEntity : class
            => new StoringService<TEntity>(CachingConfiguration.Instance).RetrieveAsync(key.ToString());

        public static Task<TEntity> RetrieveAsync<TEntity>(this string key) where TEntity : class
            => new StoringService<TEntity>(CachingConfiguration.Instance).RetrieveAsync(key);

        internal static Task<TEntity> RetrieveAsync<TEntity>(this string key, CachingConfigurationBase configuration) where TEntity : class
            => new StoringService<TEntity>(configuration).RetrieveAsync(key);

        internal static Task<TEntity> RetrieveAsync<TEntity>(this object key, CachingConfigurationBase configuration) where TEntity : class
            => new StoringService<TEntity>(configuration).RetrieveAsync(key);

        internal static Task<TEntity> RetrieveAsync<TEntity>(this int key, CachingConfigurationBase configuration) where TEntity : class
            => new StoringService<TEntity>(configuration).RetrieveAsync(key.ToString());

        #endregion

        #region RemoveAsync

        public static Task RemoveAsync<TEntity>(this int key) where TEntity : class
            => new StoringService<TEntity>(CachingConfiguration.Instance).RemoveAsync(key.ToString());

        public static Task RemoveAsync<TEntity>(this bool key) where TEntity : class
            => new StoringService<TEntity>(CachingConfiguration.Instance).RemoveAsync(key.ToString());

        public static Task RemoveAsync<TEntity>(this byte key) where TEntity : class
            => new StoringService<TEntity>(CachingConfiguration.Instance).RemoveAsync(key.ToString());

        public static Task RemoveAsync<TEntity>(this sbyte key) where TEntity : class
            => new StoringService<TEntity>(CachingConfiguration.Instance).RemoveAsync(key.ToString());

        public static Task RemoveAsync<TEntity>(this char key) where TEntity : class
            => new StoringService<TEntity>(CachingConfiguration.Instance).RemoveAsync(key.ToString());

        public static Task RemoveAsync<TEntity>(this decimal key) where TEntity : class
            => new StoringService<TEntity>(CachingConfiguration.Instance).RemoveAsync(key.ToString());

        public static Task RemoveAsync<TEntity>(this double key) where TEntity : class
            => new StoringService<TEntity>(CachingConfiguration.Instance).RemoveAsync(key.ToString());

        public static Task RemoveAsync<TEntity>(this float key) where TEntity : class
            => new StoringService<TEntity>(CachingConfiguration.Instance).RemoveAsync(key.ToString());

        public static Task RemoveAsync<TEntity>(this uint key) where TEntity : class
            => new StoringService<TEntity>(CachingConfiguration.Instance).RemoveAsync(key.ToString());

        public static Task RemoveAsync<TEntity>(this long key) where TEntity : class
            => new StoringService<TEntity>(CachingConfiguration.Instance).RemoveAsync(key.ToString());

        public static Task RemoveAsync<TEntity>(this ulong key) where TEntity : class
            => new StoringService<TEntity>(CachingConfiguration.Instance).RemoveAsync(key.ToString());

        public static Task RemoveAsync<TEntity>(this short key) where TEntity : class
            => new StoringService<TEntity>(CachingConfiguration.Instance).RemoveAsync(key.ToString());

        public static Task RemoveAsync<TEntity>(this ushort key) where TEntity : class
            => new StoringService<TEntity>(CachingConfiguration.Instance).RemoveAsync(key.ToString());

        public static Task RemoveAsync<TEntity>(this string key) where TEntity : class
            => new StoringService<TEntity>(CachingConfiguration.Instance).RemoveAsync(key);

        internal static Task RemoveAsync<TEntity>(this string key, CachingConfigurationBase configuration) where TEntity : class
            => new StoringService<TEntity>(configuration).RemoveAsync(key);

        internal static Task RemoveAsync<TEntity>(this object key, CachingConfigurationBase configuration) where TEntity : class
            => new StoringService<TEntity>(configuration).RemoveAsync(key);

        internal static Task RemoveAsync<TEntity>(this int key, CachingConfigurationBase configuration) where TEntity : class
            => new StoringService<TEntity>(configuration).RemoveAsync(key.ToString());

        #endregion

        public static T Or<T>(this T value) => value;
    }
}
