using System;
using FluentCaching.Cache.Models;

namespace FluentCaching.PolicyBuilders.Ttl
{
    public class InfiniteTtlBuilder
    {
        private CacheOptions _currentOptions;

        public InfiniteTtlBuilder(CacheOptions currentOptions)
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
