using System;
using FluentCaching.Keys.Builders;

namespace FluentCaching.Cache.Models
{
    public class CacheOptions
    {
        public TimeSpan Ttl { get; set; }

        public ExpirationType ExpirationType { get; set; }

        internal IKeyBuilder KeyBuilder { get; set; }

        internal ICacheImplementation CacheImplementation { get; set; }
    }
}
