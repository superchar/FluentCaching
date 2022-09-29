using System;
using System.Threading.Tasks;
using FluentCaching.Cache.Facades;

namespace FluentCaching.Cache
{
    internal class Cache : ICache
    {
        private readonly ICacheFacade _cacheFacade;

        public Cache(ICacheFacade cacheFacade)
        {
            _cacheFacade = cacheFacade;
        }

        #region StoreAsync

        public Task CacheAsync<TEntity>(TEntity targetObject) where TEntity : class
           => _cacheFacade.CacheAsync(targetObject);

        #endregion

        #region RetrieveAsync 

        public Task<TEntity> RetrieveAsync<TEntity>(object key) where TEntity : class
            => _cacheFacade.RetrieveComplexAsync<TEntity>(key);

        public Task<TEntity> RetrieveAsync<TEntity>(int key) where TEntity : class
            => _cacheFacade.RetrieveScalarAsync<TEntity>(key);

        public Task<TEntity> RetrieveAsync<TEntity>(bool key) where TEntity : class
            => _cacheFacade.RetrieveScalarAsync<TEntity>(key);

        public Task<TEntity> RetrieveAsync<TEntity>(byte key) where TEntity : class
            => _cacheFacade.RetrieveScalarAsync<TEntity>(key);

        public Task<TEntity> RetrieveAsync<TEntity>(sbyte key) where TEntity : class
            => _cacheFacade.RetrieveScalarAsync<TEntity>(key);

        public Task<TEntity> RetrieveAsync<TEntity>(char key) where TEntity : class
            => _cacheFacade.RetrieveScalarAsync<TEntity>(key);

        public Task<TEntity> RetrieveAsync<TEntity>(decimal key) where TEntity : class
            => _cacheFacade.RetrieveScalarAsync<TEntity>(key);

        public Task<TEntity> RetrieveAsync<TEntity>(double key) where TEntity : class
            => _cacheFacade.RetrieveScalarAsync<TEntity>(key);

        public Task<TEntity> RetrieveAsync<TEntity>(float key) where TEntity : class
            => _cacheFacade.RetrieveScalarAsync<TEntity>(key);

        public Task<TEntity> RetrieveAsync<TEntity>(uint key) where TEntity : class
            => _cacheFacade.RetrieveScalarAsync<TEntity>(key);

        public Task<TEntity> RetrieveAsync<TEntity>(long key) where TEntity : class
            => _cacheFacade.RetrieveScalarAsync<TEntity>(key);

        public Task<TEntity> RetrieveAsync<TEntity>(ulong key) where TEntity : class
            => _cacheFacade.RetrieveScalarAsync<TEntity>(key);

        public Task<TEntity> RetrieveAsync<TEntity>(short key) where TEntity : class
            => _cacheFacade.RetrieveScalarAsync<TEntity>(key);

        public Task<TEntity> RetrieveAsync<TEntity>(ushort key) where TEntity : class
            => _cacheFacade.RetrieveScalarAsync<TEntity>(key);

        public Task<TEntity> RetrieveAsync<TEntity>(string key) where TEntity : class
            => _cacheFacade.RetrieveScalarAsync<TEntity>(key);
        
        public Task<TEntity> RetrieveAsync<TEntity>(Guid key) where TEntity : class
            => _cacheFacade.RetrieveScalarAsync<TEntity>(key);

        public Task<TEntity> RetrieveAsync<TEntity>() where TEntity : class
            => _cacheFacade.RetrieveStaticAsync<TEntity>();

        #endregion

        #region RemoveAsync

        public Task RemoveAsync<TEntity>(int key) where TEntity : class
            => _cacheFacade.RemoveScalarAsync<TEntity>(key);

        public Task RemoveAsync<TEntity>(bool key) where TEntity : class
            => _cacheFacade.RemoveScalarAsync<TEntity>(key);

        public Task RemoveAsync<TEntity>(byte key) where TEntity : class
            => _cacheFacade.RemoveScalarAsync<TEntity>(key);

        public Task RemoveAsync<TEntity>(sbyte key) where TEntity : class
            => _cacheFacade.RemoveScalarAsync<TEntity>(key);

        public Task RemoveAsync<TEntity>(char key) where TEntity : class
            => _cacheFacade.RemoveScalarAsync<TEntity>(key);

        public Task RemoveAsync<TEntity>(decimal key) where TEntity : class
            => _cacheFacade.RemoveScalarAsync<TEntity>(key);

        public Task RemoveAsync<TEntity>(double key) where TEntity : class
            => _cacheFacade.RemoveScalarAsync<TEntity>(key);

        public Task RemoveAsync<TEntity>(float key) where TEntity : class
            => _cacheFacade.RemoveScalarAsync<TEntity>(key);

        public Task RemoveAsync<TEntity>(uint key) where TEntity : class
            => _cacheFacade.RemoveScalarAsync<TEntity>(key);

        public Task RemoveAsync<TEntity>(long key) where TEntity : class
            => _cacheFacade.RemoveScalarAsync<TEntity>(key);

        public Task RemoveAsync<TEntity>(ulong key) where TEntity : class
            => _cacheFacade.RemoveScalarAsync<TEntity>(key);

        public Task RemoveAsync<TEntity>(short key) where TEntity : class
            => _cacheFacade.RemoveScalarAsync<TEntity>(key);

        public Task RemoveAsync<TEntity>(ushort key) where TEntity : class
            => _cacheFacade.RemoveScalarAsync<TEntity>(key);

        public Task RemoveAsync<TEntity>(string key) where TEntity : class
            => _cacheFacade.RemoveScalarAsync<TEntity>(key);
        
        public Task RemoveAsync<TEntity>(Guid key) where TEntity : class
            => _cacheFacade.RemoveScalarAsync<TEntity>(key);

        public Task RemoveAsync<TEntity>(object key) where TEntity : class
            => _cacheFacade.RemoveComplexAsync<TEntity>(key);

        public Task RemoveAsync<TEntity>() where TEntity : class
            => _cacheFacade.RemoveStaticAsync<TEntity>();

        #endregion
    }
}
