using System;
using FluentCaching.Cache.Models;
using FluentCaching.Keys;
using FluentCaching.Keys.Builders;

namespace FluentCaching.PolicyBuilders.Ttl
{
    public class TtlTypeBuilder
    {
        private readonly CacheOptions _currentOptions = new ();

        internal TtlTypeBuilder(IKeyBuilder keyBuilder)
        {
            _currentOptions.KeyBuilder = keyBuilder;
        }

        public TimeTtlBuilder WithTtlOf(ushort value) => new (_currentOptions, value);

        public AndBuilder<CacheImplementationBuilder> WithInfiniteTtl()
        {
            _currentOptions.Ttl = TimeSpan.MaxValue;
            return new AndBuilder<CacheImplementationBuilder>(new CacheImplementationBuilder(_currentOptions));
        }
    }
}
