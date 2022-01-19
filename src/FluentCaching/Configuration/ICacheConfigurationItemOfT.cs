
using FluentCaching.Keys;

namespace FluentCaching.Configuration
{
    internal interface ICacheConfigurationItem<T> : ICacheConfigurationItem 
        where T : class
    {
        IPropertyTracker<T> Tracker { get; }
    }
}
