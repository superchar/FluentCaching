using FluentCaching.Cache.Models;
using FluentCaching.Keys;
using FluentCaching.Keys.Builders;

namespace FluentCaching.Configuration
{
    internal class CacheConfigurationItem : ICacheConfigurationItem
    {
        public CacheConfigurationItem(CacheOptions options)
        {
            Options = options;
        }
        
        public CacheOptions Options { get; }
    }
}
