using FluentCaching.Keys;
using FluentCaching.Parameters;

namespace FluentCaching.Configuration
{
    internal class CachingConfigurationItem<T> : ICachingConfigurationItem where T : class
    {
        public CachingConfigurationItem(CachingOptions options)
        {
            Tracker = options.PropertyTracker as PropertyTracker<T>;
            Options = options;
        }

        public PropertyTracker<T> Tracker { get; }

        public CachingOptions Options { get; }
    }
}
