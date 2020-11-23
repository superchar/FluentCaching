

using System;
using FluentCaching.Api.Ttl;
using FluentCaching.Parameters;

namespace FluentCaching.Api
{
    public class CachingOptionsBuilder
    {
        private readonly CachingOptions _currentOptions = CachingOptions.Default;

        public CachingOptionsBuilder(string key)
        {
            _currentOptions.Key = key;
        }

        public TtlBuilder WithTtlOf(short value)
        {
            return new TtlBuilder(_currentOptions, value);
        }
    }
}
