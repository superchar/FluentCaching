using FluentCaching.Cache.Models;

namespace FluentCaching.Configuration
{
    internal interface ICacheConfigurationItem
    {
        CacheOptions Options { get; }
    }
}
