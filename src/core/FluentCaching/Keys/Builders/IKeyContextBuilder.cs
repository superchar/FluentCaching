using FluentCaching.Keys.Models;

namespace FluentCaching.Keys.Builders;

internal interface IKeyContextBuilder
{
    void AddKey(string key);
        
    KeyContext BuildCacheContext(object cachedObject);
        
    KeyContext BuildRetrieveContextFromScalarKey(object scalarKey);
        
    KeyContext BuildRetrieveContextFromComplexKey(object complexKey);
}