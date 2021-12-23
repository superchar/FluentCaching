using FluentCaching.Cache.Models;
using FluentCaching.Keys;

namespace FluentCaching.Configuration
{
    internal class CacheConfigurationItem<T> : ICacheConfigurationItem<T> where T : class
    {
        public CacheConfigurationItem(CacheOptions options)
        {
            Tracker = options.PropertyTracker as PropertyTracker<T>;
            Options = options;
        }

        public IPropertyTracker<T> Tracker { get; }

        public CacheOptions Options { get; }
    }
}
