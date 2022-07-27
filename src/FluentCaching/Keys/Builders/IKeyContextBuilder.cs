using System.Collections.Generic;

namespace FluentCaching.Keys.Builders
{
    internal interface IKeyContextBuilder
    {
        void AddKey(string key);
    
        IDictionary<string, object> BuildKeyContextFromObject(object targetObject);
        
        IDictionary<string, object> BuildKeyContextFromString(string targetString);
    }
}