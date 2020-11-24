using System;
using System.Collections.Generic;
using System.Text;
using FluentCaching.Api;
using FluentCaching.Api.Keys;
using FluentCaching.Keys;

namespace FluentCaching.Configuration
{
    internal class CachingConfigurationItem<T> : CachingConfigurationItem where T: class
    {
        public CachingConfigurationItem(PropertyTracker tracker, 
            Func<CachingKeyBuilder<T>, ExpirationBuilder> factory) : base(tracker)
        {
            Factory = factory;
        }

        public Func<CachingKeyBuilder<T>, ExpirationBuilder> Factory { get; }
    }
}
