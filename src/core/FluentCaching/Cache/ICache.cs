using System;
using System.Threading.Tasks;

namespace FluentCaching.Cache
{
    public interface ICache
    {
        #region CacheAsync 
        
        ValueTask CacheAsync<TEntity>(TEntity targetObject) where TEntity : class;

        #endregion CacheAsync

        #region RemoveAsync

        ValueTask RemoveAsync<TEntity>(bool key) where TEntity : class;
       
        ValueTask RemoveAsync<TEntity>(byte key) where TEntity : class;
       
        ValueTask RemoveAsync<TEntity>(char key) where TEntity : class;
      
        ValueTask RemoveAsync<TEntity>(decimal key) where TEntity : class;
       
        ValueTask RemoveAsync<TEntity>(double key) where TEntity : class;
     
        ValueTask RemoveAsync<TEntity>(float key) where TEntity : class;
       
        ValueTask RemoveAsync<TEntity>(int key) where TEntity : class;
      
        ValueTask RemoveAsync<TEntity>(long key) where TEntity : class;
      
        ValueTask RemoveAsync<TEntity>(sbyte key) where TEntity : class;
       
        ValueTask RemoveAsync<TEntity>(short key) where TEntity : class;
       
        ValueTask RemoveAsync<TEntity>(string key) where TEntity : class;
      
        ValueTask RemoveAsync<TEntity>(uint key) where TEntity : class;
       
        ValueTask RemoveAsync<TEntity>(ulong key) where TEntity : class;
        
        ValueTask RemoveAsync<TEntity>(ushort key) where TEntity : class;
        
        ValueTask RemoveAsync<TEntity>(Guid key) where TEntity : class;

        ValueTask RemoveAsync<TEntity>(object key) where TEntity : class;

        ValueTask RemoveAsync<TEntity>() where TEntity : class;

        #endregion

        #region RetrieveAsync

        ValueTask<TEntity> RetrieveAsync<TEntity>(bool key) where TEntity : class;
        
        ValueTask<TEntity> RetrieveAsync<TEntity>(byte key) where TEntity : class;
        
        ValueTask<TEntity> RetrieveAsync<TEntity>(char key) where TEntity : class;
        
        ValueTask<TEntity> RetrieveAsync<TEntity>(decimal key) where TEntity : class;
       
        ValueTask<TEntity> RetrieveAsync<TEntity>(double key) where TEntity : class;
        
        ValueTask<TEntity> RetrieveAsync<TEntity>(float key) where TEntity : class;
        
        ValueTask<TEntity> RetrieveAsync<TEntity>(int key) where TEntity : class;
        
        ValueTask<TEntity> RetrieveAsync<TEntity>(long key) where TEntity : class;
        
        ValueTask<TEntity> RetrieveAsync<TEntity>(object key) where TEntity : class;
        
        ValueTask<TEntity> RetrieveAsync<TEntity>(sbyte key) where TEntity : class;
        
        ValueTask<TEntity> RetrieveAsync<TEntity>(short key) where TEntity : class;
        
        ValueTask<TEntity> RetrieveAsync<TEntity>(string key) where TEntity : class;
        
        ValueTask<TEntity> RetrieveAsync<TEntity>(uint key) where TEntity : class;
        
        ValueTask<TEntity> RetrieveAsync<TEntity>(ulong key) where TEntity : class;
        
        ValueTask<TEntity> RetrieveAsync<TEntity>(ushort key) where TEntity : class;

        ValueTask<TEntity> RetrieveAsync<TEntity>() where TEntity : class;
        
        ValueTask<TEntity> RetrieveAsync<TEntity>(Guid key) where TEntity : class;

        #endregion
    }
}