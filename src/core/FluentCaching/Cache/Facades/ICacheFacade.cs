using System.Threading.Tasks;

namespace FluentCaching.Cache.Facades
{
    internal interface ICacheFacade 
    {
        ValueTask CacheAsync<T>(T targetObject) where T : class;
        
        ValueTask<T> RetrieveComplexAsync<T>(object complexKey) where T : class;

        ValueTask<T> RetrieveScalarAsync<T>(object stringKey) where T : class;

        ValueTask<T> RetrieveStaticAsync<T>() where T : class;
        
        ValueTask RemoveComplexAsync<T>(object objectKey) where T : class;

        ValueTask RemoveScalarAsync<T>(object scalarKey) where T : class;
        
        ValueTask RemoveStaticAsync<T>() where T : class;
    }
}