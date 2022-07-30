using System.Collections.Generic;
using FluentCaching.Keys.Models;

namespace FluentCaching.Keys.Builders
{
    internal interface IKeyContextBuilder<T>
        where T : class
    {
        void AddKey(string key);
    
        KeyContext<T> BuildRetrieveContextFromObjectKey(object targetObject);
        
        KeyContext<T> BuildRetrieveContextFromStringKey(string targetString);
        
        KeyContext<T> BuildCacheContext(T cachedObject);
    }
}