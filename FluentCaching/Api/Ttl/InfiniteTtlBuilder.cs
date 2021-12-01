using System;
using FluentCaching.Parameters;

namespace FluentCaching.Api.Ttl
{
    public class InfiniteTtlBuilder
    {
        private CachingOptions _currentOptions;

        public InfiniteTtlBuilder(CachingOptions currentOptions)
        {
            _currentOptions = currentOptions;
        }

        public CacheImplementationBuilder And()
        {
            _currentOptions.Ttl = TimeSpan.MaxValue;
            return new CacheImplementationBuilder(_currentOptions);
        }
    }
}
