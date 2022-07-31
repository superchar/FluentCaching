using FluentCaching.Cache.Models;
using FluentCaching.Keys.Builders;

namespace FluentCaching.Configuration
{
    internal class CacheConfigurationItem<T> : ICacheConfigurationItem<T> where T : class
    {
        public CacheConfigurationItem(CacheOptions options)
        {
            KeyBuilder = options.KeyBuilder as KeyBuilder<T>;
            Options = options;
        }

        public IKeyBuilder<T> KeyBuilder { get; }

        public CacheOptions Options { get; }
    }
}
