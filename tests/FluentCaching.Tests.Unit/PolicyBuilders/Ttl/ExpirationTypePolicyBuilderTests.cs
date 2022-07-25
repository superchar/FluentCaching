using FluentAssertions;
using FluentCaching.Cache.Models;
using FluentCaching.PolicyBuilders.Ttl;
using Xunit;

namespace FluentCaching.Tests.Unit.PolicyBuilders.Ttl
{
    public class ExpirationTypePolicyBuilderTests
    {
        private readonly CacheOptions _cacheOptions;

        private readonly ExpirationTypePolicyBuilder _sut;

        public ExpirationTypePolicyBuilderTests()
        {
            _cacheOptions = new CacheOptions();

            _sut = new ExpirationTypePolicyBuilder(_cacheOptions);
        }

        [Fact]
        public void AbsoluteExpiration_HappyPath_SetsAbsoluteExpirationType()
        {
            var result = _sut.AbsoluteExpiration();

            result.Should().NotBeNull();  
            _cacheOptions.ExpirationType.Should().Be(ExpirationType.Absolute);
        }

        [Fact]
        public void SlidingExpiration_HappyPath_SetsSlidingExpirationType()
        {
            var result = _sut.SlidingExpiration();

            result.Should().NotBeNull();
            _cacheOptions.ExpirationType.Should().Be(ExpirationType.Sliding);
        }
    }
}
