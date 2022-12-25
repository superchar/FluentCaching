using System;
using System.Threading.Tasks;

namespace FluentCaching.Cache
{
    public interface ICache
    {
        #region CacheAsync 
        
        Task CacheAsync<TEntity>(TEntity targetObject) where TEntity : class;

        #endregion StoreAsync

        #region RemoveAsync

        Task RemoveAsync<TEntity>(bool key) where TEntity : class;
       
        Task RemoveAsync<TEntity>(byte key) where TEntity : class;
       
        Task RemoveAsync<TEntity>(char key) where TEntity : class;
      
        Task RemoveAsync<TEntity>(decimal key) where TEntity : class;
       
        Task RemoveAsync<TEntity>(double key) where TEntity : class;
     
        Task RemoveAsync<TEntity>(float key) where TEntity : class;
       
        Task RemoveAsync<TEntity>(int key) where TEntity : class;
      
        Task RemoveAsync<TEntity>(long key) where TEntity : class;
      
        Task RemoveAsync<TEntity>(sbyte key) where TEntity : class;
       
        Task RemoveAsync<TEntity>(short key) where TEntity : class;
       
        Task RemoveAsync<TEntity>(string key) where TEntity : class;
      
        Task RemoveAsync<TEntity>(uint key) where TEntity : class;
       
        Task RemoveAsync<TEntity>(ulong key) where TEntity : class;
        
        Task RemoveAsync<TEntity>(ushort key) where TEntity : class;
        
        Task RemoveAsync<TEntity>(Guid key) where TEntity : class;

        Task RemoveAsync<TEntity>(object key) where TEntity : class;

        Task RemoveAsync<TEntity>() where TEntity : class;

        #endregion

        #region RetrieveAsync

        Task<TEntity> RetrieveAsync<TEntity>(bool key) where TEntity : class;
        
        Task<TEntity> RetrieveAsync<TEntity>(byte key) where TEntity : class;
        
        Task<TEntity> RetrieveAsync<TEntity>(char key) where TEntity : class;
        
        Task<TEntity> RetrieveAsync<TEntity>(decimal key) where TEntity : class;
       
        Task<TEntity> RetrieveAsync<TEntity>(double key) where TEntity : class;
        
        Task<TEntity> RetrieveAsync<TEntity>(float key) where TEntity : class;
        
        Task<TEntity> RetrieveAsync<TEntity>(int key) where TEntity : class;
        
        Task<TEntity> RetrieveAsync<TEntity>(long key) where TEntity : class;
        
        Task<TEntity> RetrieveAsync<TEntity>(object key) where TEntity : class;
        
        Task<TEntity> RetrieveAsync<TEntity>(sbyte key) where TEntity : class;
        
        Task<TEntity> RetrieveAsync<TEntity>(short key) where TEntity : class;
        
        Task<TEntity> RetrieveAsync<TEntity>(string key) where TEntity : class;
        
        Task<TEntity> RetrieveAsync<TEntity>(uint key) where TEntity : class;
        
        Task<TEntity> RetrieveAsync<TEntity>(ulong key) where TEntity : class;
        
        Task<TEntity> RetrieveAsync<TEntity>(ushort key) where TEntity : class;

        Task<TEntity> RetrieveAsync<TEntity>() where TEntity : class;
        
        Task<TEntity> RetrieveAsync<TEntity>(Guid key) where TEntity : class;

        #endregion
    }
}