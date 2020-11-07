
using System;

namespace FluentCaching.Configuration
{
    public class CachingConfiguration
    {
        public static CachingConfiguration Instance = new CachingConfiguration();

        private CachingConfiguration()
        {

        }

        public ICacheImplementation Current { get; private set; }

        public void SetImplementation(ICacheImplementation cacheImplementation)
        {
            if (Current != null)
            {
                throw new ArgumentException("Cache implementation is already set", nameof(cacheImplementation));
            }

            Current = cacheImplementation;
        }
    }
}
