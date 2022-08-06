using FluentCaching.Keys.Models;

namespace FluentCaching.Keys.Builders
{
    internal interface IKeyContextBuilder
    {
        void AddKey(string key);
    
        KeyContext BuildRetrieveContextFromObjectKey(object targetObject);
        
        KeyContext BuildRetrieveContextFromStringKey(string targetString);
        
        KeyContext BuildCacheContext(object cachedObject);
    }
}