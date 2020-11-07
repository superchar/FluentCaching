

using System;
using FluentCaching.Api.Ttl;
using FluentCaching.Parameters;

namespace FluentCaching.Api
{
    public class CachingOptionsBuilder
    {
        private readonly CachingOptions _currentOptions = CachingOptions.Default;

        public CachingOptionsBuilder(string key, object targetObject)
        {
            _currentOptions.Key = key;
            _currentOptions.TargetObject = targetObject;
        }

        public TtlBuilder WithTtlOf(short value)
        {
            return new TtlBuilder(_currentOptions, value);
        }
    }
}
