
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using FluentCaching.Api;
using FluentCaching.Api.Keys;

namespace FluentCaching.Configuration
{
    public class CachingConfiguration
    {
        public static readonly  CachingConfiguration Instance = new CachingConfiguration();

        private readonly Dictionary<Type, CachingConfigurationItem> _predefinedConfigurations = //Should be thread safe when readonly (mutations present only at configuration phase)
            new Dictionary<Type, CachingConfigurationItem>();

        private CachingConfiguration()
        {

        }

        internal ICacheImplementation Current { get; private set; }

        internal void SetImplementation(ICacheImplementation cacheImplementation)
        {
            if (Current != null)
            {
                throw new ArgumentException("Cache implementation is already set", nameof(cacheImplementation));
            }

            Current = cacheImplementation;
        }

        public void For<T>(Func<CachingKeyBuilder<T>, ExpirationBuilder> factoryFunc)
            where T : class
        {
            var tracker = factoryFunc(CachingKeyBuilder<T>.Empty)
                .CachingOptions
                .PropertyTracker;

            _predefinedConfigurations[typeof(T)] = new CachingConfigurationItem<T>(tracker, factoryFunc);
        }

        internal Func<CachingKeyBuilder<T>, ExpirationBuilder> GetFactory<T>()
            where T : class

        {
            return ((CachingConfigurationItem<T>) _predefinedConfigurations[typeof(T)]).Factory;
        }

        internal CachingConfigurationItem<T> GetItem<T>()
            where T : class

        {
            return ((CachingConfigurationItem<T>)_predefinedConfigurations[typeof(T)]);
        }

        internal void Reset()
        {
            _predefinedConfigurations.Clear();
            Current = null;
        }
    }
}
