using System;
using FluentAssertions;
using FluentCaching.Configuration.PolicyBuilders.Ttl;
using FluentCaching.Keys.Builders;
using Moq;
using Xunit;

namespace FluentCaching.Tests.Unit.Configuration.PolicyBuilders.Ttl
{
    public class TtlTypePolicyBuilderTests
    {
        private readonly TtlTypePolicyBuilder _sut;

        public TtlTypePolicyBuilderTests()
        {
            var keyBuilderMock = new Mock<IKeyBuilder>();

            _sut = new TtlTypePolicyBuilder(keyBuilderMock.Object);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(42)]
        [InlineData(ushort.MaxValue)]
        public void WithTtlOf_HappyPath_ReturnsTtlTypeBuilder(ushort ttl)
        {
            var result = _sut.WithTtlOf(ttl);

            result.Should().NotBeNull();
        }

        [Fact]
        public void WithInfiniteTtl_HappyPath_ReturnsAndBuilderWithMaxValueTtl()
        {
            var result = _sut.WithInfiniteTtl();

            result.Should().NotBeNull();
            var cacheOptions = result.And()?.CachingOptions;
            cacheOptions.Should().NotBeNull();
            cacheOptions.Ttl.Should().Be(TimeSpan.MaxValue);
        }
    }
}
