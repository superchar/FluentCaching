using System;

namespace FluentCaching.Configuration.Exceptions
{
    public class CacheImplementationNotFoundException : Exception
    {
        public CacheImplementationNotFoundException(Type type) 
            : base($"No caching implementation configured for type - '{type.FullName}'")
        {

        }
    }
}
