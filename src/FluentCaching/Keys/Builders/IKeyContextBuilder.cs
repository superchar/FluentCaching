using System.Collections.Generic;

namespace FluentCaching.Keys.Builders
{
    internal interface IKeyContextBuilder
    {
        IDictionary<string, object> BuildKeyContextFromObject(object targetObject);
        
        IDictionary<string, object> BuildKeyContextFromString(string targetString);
    }
}