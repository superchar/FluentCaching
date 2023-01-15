using System;
using System.Threading.Tasks;
using FluentCaching.Cache.Facades;

namespace FluentCaching.Cache;

internal class Cache : ICache
{
    private readonly ICacheFacade _cacheFacade;

    public Cache(ICacheFacade cacheFacade)
    {
        _cacheFacade = cacheFacade;
    }

    #region StoreAsync

    public ValueTask CacheAsync<TEntity>(TEntity targetObject) where TEntity : class
        => _cacheFacade.CacheAsync(targetObject);

    #endregion

    #region RetrieveAsync 

    public ValueTask<TEntity> RetrieveAsync<TEntity>(object key) where TEntity : class
        => _cacheFacade.RetrieveComplexAsync<TEntity>(key);

    public ValueTask<TEntity> RetrieveAsync<TEntity>(int key) where TEntity : class
        => _cacheFacade.RetrieveScalarAsync<TEntity>(key);

    public ValueTask<TEntity> RetrieveAsync<TEntity>(bool key) where TEntity : class
        => _cacheFacade.RetrieveScalarAsync<TEntity>(key);

    public ValueTask<TEntity> RetrieveAsync<TEntity>(byte key) where TEntity : class
        => _cacheFacade.RetrieveScalarAsync<TEntity>(key);

    public ValueTask<TEntity> RetrieveAsync<TEntity>(sbyte key) where TEntity : class
        => _cacheFacade.RetrieveScalarAsync<TEntity>(key);

    public ValueTask<TEntity> RetrieveAsync<TEntity>(char key) where TEntity : class
        => _cacheFacade.RetrieveScalarAsync<TEntity>(key);

    public ValueTask<TEntity> RetrieveAsync<TEntity>(decimal key) where TEntity : class
        => _cacheFacade.RetrieveScalarAsync<TEntity>(key);

    public ValueTask<TEntity> RetrieveAsync<TEntity>(double key) where TEntity : class
        => _cacheFacade.RetrieveScalarAsync<TEntity>(key);

    public ValueTask<TEntity> RetrieveAsync<TEntity>(float key) where TEntity : class
        => _cacheFacade.RetrieveScalarAsync<TEntity>(key);

    public ValueTask<TEntity> RetrieveAsync<TEntity>(uint key) where TEntity : class
        => _cacheFacade.RetrieveScalarAsync<TEntity>(key);

    public ValueTask<TEntity> RetrieveAsync<TEntity>(long key) where TEntity : class
        => _cacheFacade.RetrieveScalarAsync<TEntity>(key);

    public ValueTask<TEntity> RetrieveAsync<TEntity>(ulong key) where TEntity : class
        => _cacheFacade.RetrieveScalarAsync<TEntity>(key);

    public ValueTask<TEntity> RetrieveAsync<TEntity>(short key) where TEntity : class
        => _cacheFacade.RetrieveScalarAsync<TEntity>(key);

    public ValueTask<TEntity> RetrieveAsync<TEntity>(ushort key) where TEntity : class
        => _cacheFacade.RetrieveScalarAsync<TEntity>(key);

    public ValueTask<TEntity> RetrieveAsync<TEntity>(string key) where TEntity : class
        => _cacheFacade.RetrieveScalarAsync<TEntity>(key);
        
    public ValueTask<TEntity> RetrieveAsync<TEntity>(Guid key) where TEntity : class
        => _cacheFacade.RetrieveScalarAsync<TEntity>(key);

    public ValueTask<TEntity> RetrieveAsync<TEntity>() where TEntity : class
        => _cacheFacade.RetrieveStaticAsync<TEntity>();

    #endregion

    #region RemoveAsync

    public ValueTask RemoveAsync<TEntity>(int key) where TEntity : class
        => _cacheFacade.RemoveScalarAsync<TEntity>(key);

    public ValueTask RemoveAsync<TEntity>(bool key) where TEntity : class
        => _cacheFacade.RemoveScalarAsync<TEntity>(key);

    public ValueTask RemoveAsync<TEntity>(byte key) where TEntity : class
        => _cacheFacade.RemoveScalarAsync<TEntity>(key);

    public ValueTask RemoveAsync<TEntity>(sbyte key) where TEntity : class
        => _cacheFacade.RemoveScalarAsync<TEntity>(key);

    public ValueTask RemoveAsync<TEntity>(char key) where TEntity : class
        => _cacheFacade.RemoveScalarAsync<TEntity>(key);

    public ValueTask RemoveAsync<TEntity>(decimal key) where TEntity : class
        => _cacheFacade.RemoveScalarAsync<TEntity>(key);

    public ValueTask RemoveAsync<TEntity>(double key) where TEntity : class
        => _cacheFacade.RemoveScalarAsync<TEntity>(key);

    public ValueTask RemoveAsync<TEntity>(float key) where TEntity : class
        => _cacheFacade.RemoveScalarAsync<TEntity>(key);

    public ValueTask RemoveAsync<TEntity>(uint key) where TEntity : class
        => _cacheFacade.RemoveScalarAsync<TEntity>(key);

    public ValueTask RemoveAsync<TEntity>(long key) where TEntity : class
        => _cacheFacade.RemoveScalarAsync<TEntity>(key);

    public ValueTask RemoveAsync<TEntity>(ulong key) where TEntity : class
        => _cacheFacade.RemoveScalarAsync<TEntity>(key);

    public ValueTask RemoveAsync<TEntity>(short key) where TEntity : class
        => _cacheFacade.RemoveScalarAsync<TEntity>(key);

    public ValueTask RemoveAsync<TEntity>(ushort key) where TEntity : class
        => _cacheFacade.RemoveScalarAsync<TEntity>(key);

    public ValueTask RemoveAsync<TEntity>(string key) where TEntity : class
        => _cacheFacade.RemoveScalarAsync<TEntity>(key);
        
    public ValueTask RemoveAsync<TEntity>(Guid key) where TEntity : class
        => _cacheFacade.RemoveScalarAsync<TEntity>(key);

    public ValueTask RemoveAsync<TEntity>(object key) where TEntity : class
        => _cacheFacade.RemoveComplexAsync<TEntity>(key);

    public ValueTask RemoveAsync<TEntity>() where TEntity : class
        => _cacheFacade.RemoveStaticAsync<TEntity>();

    #endregion
}