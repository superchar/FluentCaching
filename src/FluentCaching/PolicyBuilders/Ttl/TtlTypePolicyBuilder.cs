using System;
using FluentCaching.Cache.Models;
using FluentCaching.Keys;
using FluentCaching.Keys.Builders;

namespace FluentCaching.PolicyBuilders.Ttl
{
    public class TtlTypePolicyBuilder
    {
        private readonly CacheOptions _currentOptions = new ();

        internal TtlTypePolicyBuilder(IKeyBuilder keyBuilder)
        {
            _currentOptions.KeyBuilder = keyBuilder;
        }

        public TimeTtlPolicyBuilder WithTtlOf(ushort value) => new (_currentOptions, value);

        public AndPolicyBuilder<CacheImplementationPolicyBuilder> WithInfiniteTtl()
        {
            _currentOptions.Ttl = TimeSpan.MaxValue;
            return new AndPolicyBuilder<CacheImplementationPolicyBuilder>(new CacheImplementationPolicyBuilder(_currentOptions));
        }
    }
}
