using FluentAssertions;
using FluentCaching.Cache.Models;
using FluentCaching.Configuration.PolicyBuilders.Ttl;
using Xunit;

namespace FluentCaching.Tests.Unit.Configuration.PolicyBuilders.Ttl
{
    public class TimeTtlValuePolicyBuilderTests
    {
        private readonly TimeTtlValuePolicyBuilder _sut =
            new TimeTtlValuePolicyBuilder(new TimeTtlPolicyBuilder(new CacheOptions(), 42));

        [Theory]
        [InlineData(0)]
        [InlineData(42)]
        [InlineData(ushort.MaxValue)]
        [InlineData(ushort.MinValue)]
        public void And_WhenCalled_ReturnsTimeTtlPolicyBuilder(ushort value)
        {
            var result = _sut.And(value);

            result.Should().NotBeNull();
        }
        
        [Fact]
        public void With_WhenCalled_ExpirationTypePolicyBuilder()
        {
            var result = _sut.With();

            result.Should().NotBeNull();
        }
    }
}