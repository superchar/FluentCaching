
using FluentCaching.Keys;
using FluentCaching.Keys.Builders;

namespace FluentCaching.Configuration
{
    internal interface ICacheConfigurationItem<T> : ICacheConfigurationItem 
        where T : class
    {
        IKeyBuilder<T> KeyBuilder { get; }
    }
}
