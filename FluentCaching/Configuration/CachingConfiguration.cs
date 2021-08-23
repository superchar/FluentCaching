using System;
using System.Collections.Generic;
using FluentCaching.Api;
using FluentCaching.Api.Keys;
using FluentCaching.Parameters;

namespace FluentCaching.Configuration
{
    public sealed class CachingConfiguration : CachingConfigurationBase
    {
        public static readonly CachingConfiguration Instance = new CachingConfiguration();

        private readonly Dictionary<Type, ICachingConfigurationItem> _predefinedConfigurations
            = //Should be thread safe when readonly (mutations present only at configuration phase)
            new Dictionary<Type, ICachingConfigurationItem>();

        private ICacheImplementation _cacheImplementation;

        private CachingConfiguration()
        {
        }

        internal static CachingConfiguration Create() => new CachingConfiguration();

        internal override ICacheImplementation Current => _cacheImplementation;

        public CachingConfiguration SetImplementation(ICacheImplementation cacheImplementation)
        {
            if (_cacheImplementation != null)
            {
                throw new ArgumentException("Cache implementation is already set", nameof(cacheImplementation));
            }

            _cacheImplementation = cacheImplementation;
            return this;
        }

        public CachingConfiguration For<T>(Func<CachingKeyBuilder<T>, ExpirationBuilder> factoryFunc)
            where T : class
            => For<T>(factoryFunc(new CachingKeyBuilder<T>()).CachingOptions);

        public CachingConfiguration For<T>(Func<CachingKeyBuilder<T>, CacheImplementationBuilder> factoryFunc)
            where T : class
            => For<T>(factoryFunc(new CachingKeyBuilder<T>()).CachingOptions);

        internal override CachingConfigurationItem<T> GetItem<T>() =>
            _predefinedConfigurations.TryGetValue(typeof(T), out var configurationItem)
                ? configurationItem as CachingConfigurationItem<T>
                : null;

        internal void Reset()
        {
            _predefinedConfigurations.Clear();
            _cacheImplementation = null;
        }

        private CachingConfiguration For<T>(CachingOptions options)
            where T : class
        {
            _predefinedConfigurations[typeof(T)] = new CachingConfigurationItem<T>(options);
            return this;
        }
    }
}
