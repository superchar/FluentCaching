

using FluentCaching.Api.Keys;
using FluentCaching.Keys;

namespace FluentCaching.Configuration
{
    internal abstract class CachingConfigurationItem
    {
        public PropertyTracker Tracker { get; }

        protected CachingConfigurationItem(PropertyTracker tracker)
        {
            Tracker = tracker;
        }
    }
}
