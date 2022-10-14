using System;
using FluentCaching.Cache.Models;
using FluentCaching.Keys.Builders;

namespace FluentCaching.Configuration.PolicyBuilders.Ttl
{
    public class TtlTypePolicyBuilder
    {
        private readonly CacheOptions _currentOptions = new ();

        internal TtlTypePolicyBuilder(IKeyBuilder keyBuilder)
        {
            _currentOptions.KeyBuilder = keyBuilder;
        }

        public TimeTtlPolicyBuilder SetExpirationTimeoutTo(ushort value) => new (_currentOptions, value);

        public AndPolicyBuilder<CacheImplementationPolicyBuilder> SetInfiniteExpirationTimeout()
        {
            _currentOptions.Ttl = TimeSpan.MaxValue;
            return new AndPolicyBuilder<CacheImplementationPolicyBuilder>(new CacheImplementationPolicyBuilder(_currentOptions));
        }
    }
}
