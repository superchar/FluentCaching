using System;
using FluentCaching.Keys;

namespace FluentCaching.Parameters
{
    public struct CachingOptions
    {
        public TimeSpan Ttl { get; set; }

        public ExpirationType ExpirationType { get; set; }

        internal IPropertyTracker PropertyTracker { get; set; }

        internal ICacheImplementation CacheImplementation { get; set; }
    }
}
