using FluentCaching.Cache.Models;
using FluentCaching.Keys;

namespace FluentCaching.Configuration
{
    internal class CacheConfigurationItem<T> : ICacheConfigurationItem where T : class
    {
        public CacheConfigurationItem(CacheOptions options)
        {
            Tracker = options.PropertyTracker as PropertyTracker<T>;
            Options = options;
        }

        public PropertyTracker<T> Tracker { get; }

        public CacheOptions Options { get; }
    }
}
