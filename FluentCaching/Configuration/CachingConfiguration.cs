
using System;
using System.Collections.Concurrent;
using FluentCaching.Api;
using FluentCaching.Api.Key;

namespace FluentCaching.Configuration
{
    public class CachingConfiguration
    {
        public static CachingConfiguration Instance = new CachingConfiguration();

        private readonly ConcurrentDictionary<Type, Delegate> _predefinedConfigurations = new ConcurrentDictionary<Type, Delegate>();

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

        public void ForType<T>(Func<CachingKeyBuilder<T>, StoringHelperWrapper> factoryFunc)
        {
            _predefinedConfigurations[typeof(T)] = factoryFunc;
        }

        internal Func<CachingKeyBuilder<T>, StoringHelperWrapper> GetFactory<T>()
        {
            return _predefinedConfigurations[typeof(T)] as Func<CachingKeyBuilder<T>, StoringHelperWrapper>;
        }
    }
}
