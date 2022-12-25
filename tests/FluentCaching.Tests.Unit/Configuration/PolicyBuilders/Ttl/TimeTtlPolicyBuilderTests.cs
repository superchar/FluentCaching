using FluentAssertions;
using FluentCaching.Cache.Models;
using FluentCaching.Configuration.PolicyBuilders.Ttl;
using Xunit;

namespace FluentCaching.Tests.Unit.Configuration.PolicyBuilders.Ttl
{
    public class TimeTtlPolicyBuilderTests
    {
        private readonly TimeTtlPolicyBuilder _sut = new(new CacheOptions(), 42);

        [Fact]
        public void Seconds_WhenCalled_ReturnsTimeTtlValuePolicyBuilder()
        {
            var result = _sut.Seconds;

            result.Should().NotBeNull();
        }
        
        [Fact]
        public void Minutes_WhenCalled_ReturnsTimeTtlValuePolicyBuilder()
        {
            var result = _sut.Minutes;

            result.Should().NotBeNull();
        }
        
        [Fact]
        public void Hours_WhenCalled_ReturnsTimeTtlValuePolicyBuilder()
        {
            var result = _sut.Hours;

            result.Should().NotBeNull();
        }
        
        [Fact]
        public void Days_WhenCalled_ReturnsTimeTtlValuePolicyBuilder()
        {
            var result = _sut.Days;

            result.Should().NotBeNull();
        }
    }
}