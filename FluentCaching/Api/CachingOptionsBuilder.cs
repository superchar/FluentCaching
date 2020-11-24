
using System;
using FluentCaching.Api.Ttl;
using FluentCaching.Keys;
using FluentCaching.Parameters;

namespace FluentCaching.Api
{
    public class CachingOptionsBuilder
    {
        private readonly CachingOptions _currentOptions = CachingOptions.Default;

        internal CachingOptionsBuilder(string key, PropertyTracker propertyTracker)
        {
            _currentOptions.Key = key;
            _currentOptions.PropertyTracker = propertyTracker;
        }

        public TtlBuilder WithTtlOf(short value)
        {
            return new TtlBuilder(_currentOptions, value);
        }
    }
}
