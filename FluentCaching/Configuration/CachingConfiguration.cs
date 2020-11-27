
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using FluentCaching.Api;
using FluentCaching.Api.Keys;

namespace FluentCaching.Configuration
{
    public class CachingConfiguration : CachingConfigurationBase
    {
        public static readonly CachingConfiguration Instance = new CachingConfiguration();

        private readonly Dictionary<Type, CachingConfigurationItem> _predefinedConfigurations = //Should be thread safe when readonly (mutations present only at configuration phase)
            new Dictionary<Type, CachingConfigurationItem>();

        private ICacheImplementation _cacheImplementation;

        private CachingConfiguration()
        {

        }

        internal static CachingConfiguration Create()
        {
            return new CachingConfiguration();
        }

        internal override ICacheImplementation Current => _cacheImplementation;

        internal void SetImplementation(ICacheImplementation cacheImplementation)
        {
            if (_cacheImplementation != null)
            {
                throw new ArgumentException("Cache implementation is already set", nameof(cacheImplementation));
            }

            _cacheImplementation = cacheImplementation;
        }

        public void For<T>(Func<CachingKeyBuilder<T>, ExpirationBuilder> factoryFunc)
            where T : class
        {
            var tracker = factoryFunc(new CachingKeyBuilder<T>())
                .CachingOptions
                .PropertyTracker;

            _predefinedConfigurations[typeof(T)] = new CachingConfigurationItem<T>(tracker, factoryFunc);
        }

        internal override CachingConfigurationItem<T> GetItem<T>()

        {
            if (_predefinedConfigurations.TryGetValue(typeof(T), out var configurationItem))
            {
                return configurationItem as CachingConfigurationItem<T>;
            }

            return null;
        }

        internal void Reset()
        {
            _predefinedConfigurations.Clear();
            _cacheImplementation = null;
        }
    }
}
