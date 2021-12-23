using System;
using System.Threading.Tasks;

namespace FluentCaching.Cache
{
    internal class Cache : ICache
    {
        private readonly IStoringService _storingService;

        public Cache(IStoringService storingService)
        {
            _storingService = storingService;
        }

        #region CacheAsync

        public Task CacheAsync<TEntity>(TEntity targetObject) where TEntity : class
           => _storingService.CacheAsync(targetObject);

        #endregion

        #region RetrieveAsync 

        public Task<TEntity> RetrieveAsync<TEntity>(object key) where TEntity : class
            => _storingService.RetrieveAsync<TEntity>(key);

        public Task<TEntity> RetrieveAsync<TEntity>(int key) where TEntity : class
            => _storingService.RetrieveAsync<TEntity>(key.ToString());

        public Task<TEntity> RetrieveAsync<TEntity>(bool key) where TEntity : class
            => _storingService.RetrieveAsync<TEntity>(key.ToString());

        public Task<TEntity> RetrieveAsync<TEntity>(byte key) where TEntity : class
            => _storingService.RetrieveAsync<TEntity>(key.ToString());

        public Task<TEntity> RetrieveAsync<TEntity>(sbyte key) where TEntity : class
            => _storingService.RetrieveAsync<TEntity>(key.ToString());

        public Task<TEntity> RetrieveAsync<TEntity>(char key) where TEntity : class
            => _storingService.RetrieveAsync<TEntity>(key.ToString());

        public Task<TEntity> RetrieveAsync<TEntity>(decimal key) where TEntity : class
            => _storingService.RetrieveAsync<TEntity>(key.ToString());

        public Task<TEntity> RetrieveAsync<TEntity>(double key) where TEntity : class
            => _storingService.RetrieveAsync<TEntity>(key.ToString());

        public Task<TEntity> RetrieveAsync<TEntity>(float key) where TEntity : class
            => _storingService.RetrieveAsync<TEntity>(key.ToString());

        public Task<TEntity> RetrieveAsync<TEntity>(uint key) where TEntity : class
            => _storingService.RetrieveAsync<TEntity>(key.ToString());

        public Task<TEntity> RetrieveAsync<TEntity>(long key) where TEntity : class
            => _storingService.RetrieveAsync<TEntity>(key.ToString());

        public Task<TEntity> RetrieveAsync<TEntity>(ulong key) where TEntity : class
            => _storingService.RetrieveAsync<TEntity>(key.ToString());

        public Task<TEntity> RetrieveAsync<TEntity>(short key) where TEntity : class
            => _storingService.RetrieveAsync<TEntity>(key.ToString());

        public Task<TEntity> RetrieveAsync<TEntity>(ushort key) where TEntity : class
            => _storingService.RetrieveAsync<TEntity>(key.ToString());

        public Task<TEntity> RetrieveAsync<TEntity>(string key) where TEntity : class
            => _storingService.RetrieveAsync<TEntity>(key);

        #endregion

        #region RemoveAsync

        public Task RemoveAsync<TEntity>(int key) where TEntity : class
            => _storingService.RemoveAsync<TEntity>(key.ToString());

        public Task RemoveAsync<TEntity>(bool key) where TEntity : class
            => _storingService.RemoveAsync<TEntity>(key.ToString());

        public Task RemoveAsync<TEntity>(byte key) where TEntity : class
            => _storingService.RemoveAsync<TEntity>(key.ToString());

        public Task RemoveAsync<TEntity>(sbyte key) where TEntity : class
            => _storingService.RemoveAsync<TEntity>(key.ToString());

        public Task RemoveAsync<TEntity>(char key) where TEntity : class
            => _storingService.RemoveAsync<TEntity>(key.ToString());

        public Task RemoveAsync<TEntity>(decimal key) where TEntity : class
            => _storingService.RemoveAsync<TEntity>(key.ToString());

        public Task RemoveAsync<TEntity>(double key) where TEntity : class
            => _storingService.RemoveAsync<TEntity>(key.ToString());

        public Task RemoveAsync<TEntity>(float key) where TEntity : class
            => _storingService.RemoveAsync<TEntity>(key.ToString());

        public Task RemoveAsync<TEntity>(uint key) where TEntity : class
            => _storingService.RemoveAsync<TEntity>(key.ToString());

        public Task RemoveAsync<TEntity>(long key) where TEntity : class
            => _storingService.RemoveAsync<TEntity>(key.ToString());

        public Task RemoveAsync<TEntity>(ulong key) where TEntity : class
            => _storingService.RemoveAsync<TEntity>(key.ToString());

        public Task RemoveAsync<TEntity>(short key) where TEntity : class
            => _storingService.RemoveAsync<TEntity>(key.ToString());

        public Task RemoveAsync<TEntity>(ushort key) where TEntity : class
            => _storingService.RemoveAsync<TEntity>(key.ToString());

        public Task RemoveAsync<TEntity>(string key) where TEntity : class
            => _storingService.RemoveAsync<TEntity>(key);

        public Task RemoveAsync<TEntity>(object key) where TEntity : class
            => _storingService.RemoveAsync<TEntity>(key);

        #endregion

        #region RetrieveOrStoreAsync

        public Task<T> RetrieveOrStoreAsync<T>(string key, Func<string, Task<T>> entityFetcher) where T : class
            => _storingService.RetrieveOrStoreAsync(key, entityFetcher);

        public Task<T> RetrieveOrStoreAsync<T>(string key, Func<string, T> entityFetcher) where T : class
            => _storingService.RetrieveOrStoreAsync(key, entityFetcher);

        public Task<T> RetrieveOrStoreAsync<T>(object key, Func<object, Task<T>> entityFetcher) where T : class
            => _storingService.RetrieveOrStoreAsync(key, entityFetcher);

        public Task<T> RetrieveOrStoreAsync<T>(object key, Func<object, T> entityFetcher) where T : class
            => _storingService.RetrieveOrStoreAsync(key, entityFetcher);

        public Task<T> RetrieveOrStoreAsync<T>(bool key, Func<bool, Task<T>> entityFetcher) where T : class
            => _storingService.RetrieveOrStoreAsync(key, entityFetcher);

        public Task<T> RetrieveOrStoreAsync<T>(bool key, Func<bool, T> entityFetcher) where T : class
            => _storingService.RetrieveOrStoreAsync(key, entityFetcher);

        public Task<T> RetrieveOrStoreAsync<T>(int key, Func<int, Task<T>> entityFetcher) where T : class
            => _storingService.RetrieveOrStoreAsync(key, entityFetcher);

        public Task<T> RetrieveOrStoreAsync<T>(int key, Func<int, T> entityFetcher) where T : class
            => _storingService.RetrieveOrStoreAsync(key, entityFetcher);

        public Task<T> RetrieveOrStoreAsync<T>(byte key, Func<byte, Task<T>> entityFetcher) where T : class
            => _storingService.RetrieveOrStoreAsync(key, entityFetcher);

        public Task<T> RetrieveOrStoreAsync<T>(byte key, Func<byte, T> entityFetcher) where T : class
            => _storingService.RetrieveOrStoreAsync(key, entityFetcher);

        public Task<T> RetrieveOrStoreAsync<T>(sbyte key, Func<sbyte, Task<T>> entityFetcher) where T : class
            => _storingService.RetrieveOrStoreAsync(key, entityFetcher);

        public Task<T> RetrieveOrStoreAsync<T>(sbyte key, Func<sbyte, T> entityFetcher) where T : class
            => _storingService.RetrieveOrStoreAsync(key, entityFetcher);

        public Task<T> RetrieveOrStoreAsync<T>(char key, Func<char, Task<T>> entityFetcher) where T : class
            => _storingService.RetrieveOrStoreAsync(key, entityFetcher);

        public Task<T> RetrieveOrStoreAsync<T>(char key, Func<char, T> entityFetcher) where T : class
            => _storingService.RetrieveOrStoreAsync(key, entityFetcher);

        public Task<T> RetrieveOrStoreAsync<T>(decimal key, Func<decimal, Task<T>> entityFetcher) where T : class
            => _storingService.RetrieveOrStoreAsync(key, entityFetcher);

        public Task<T> RetrieveOrStoreAsync<T>(decimal key, Func<decimal, T> entityFetcher) where T : class
            => _storingService.RetrieveOrStoreAsync(key, entityFetcher);

        public Task<T> RetrieveOrStoreAsync<T>(double key, Func<double, Task<T>> entityFetcher) where T : class
            => _storingService.RetrieveOrStoreAsync(key, entityFetcher);

        public Task<T> RetrieveOrStoreAsync<T>(double key, Func<double, T> entityFetcher) where T : class
            => _storingService.RetrieveOrStoreAsync(key, entityFetcher);

        public Task<T> RetrieveOrStoreAsync<T>(float key, Func<float, Task<T>> entityFetcher) where T : class
            => _storingService.RetrieveOrStoreAsync(key, entityFetcher);

        public Task<T> RetrieveOrStoreAsync<T>(float key, Func<float, T> entityFetcher) where T : class
            => _storingService.RetrieveOrStoreAsync(key, entityFetcher);

        public Task<T> RetrieveOrStoreAsync<T>(long key, Func<long, Task<T>> entityFetcher) where T : class
            => _storingService.RetrieveOrStoreAsync(key, entityFetcher);

        public Task<T> RetrieveOrStoreAsync<T>(long key, Func<long, T> entityFetcher) where T : class
            => _storingService.RetrieveOrStoreAsync(key, entityFetcher);

        public Task<T> RetrieveOrStoreAsync<T>(ulong key, Func<ulong, Task<T>> entityFetcher) where T : class
            => _storingService.RetrieveOrStoreAsync(key, entityFetcher);

        public Task<T> RetrieveOrStoreAsync<T>(ulong key, Func<ulong, T> entityFetcher) where T : class
            => _storingService.RetrieveOrStoreAsync(key, entityFetcher);

        public Task<T> RetrieveOrStoreAsync<T>(short key, Func<short, Task<T>> entityFetcher) where T : class
           => _storingService.RetrieveOrStoreAsync(key, entityFetcher);

        public Task<T> RetrieveOrStoreAsync<T>(short key, Func<short, T> entityFetcher) where T : class
            => _storingService.RetrieveOrStoreAsync(key, entityFetcher);

        public Task<T> RetrieveOrStoreAsync<T>(ushort key, Func<ushort, Task<T>> entityFetcher) where T : class
           => _storingService.RetrieveOrStoreAsync(key, entityFetcher);

        public Task<T> RetrieveOrStoreAsync<T>(ushort key, Func<ushort, T> entityFetcher) where T : class
            => _storingService.RetrieveOrStoreAsync(key, entityFetcher);

        public Task<T> RetrieveOrStoreAsync<T>(uint key, Func<uint, Task<T>> entityFetcher) where T : class
            => _storingService.RetrieveOrStoreAsync(key, entityFetcher);

        public Task<T> RetrieveOrStoreAsync<T>(uint key, Func<uint, T> entityFetcher) where T : class
            => _storingService.RetrieveOrStoreAsync(key, entityFetcher);

        #endregion
    }
}
