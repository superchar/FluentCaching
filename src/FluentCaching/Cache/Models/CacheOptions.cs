using System;
using FluentCaching.Keys;

namespace FluentCaching.Cache.Models
{
    public class CacheOptions
    {
        public TimeSpan Ttl { get; set; }

        public ExpirationType ExpirationType { get; set; }

        internal IPropertyTracker PropertyTracker { get; set; }

        internal ICacheImplementation CacheImplementation { get; set; }
    }
}
