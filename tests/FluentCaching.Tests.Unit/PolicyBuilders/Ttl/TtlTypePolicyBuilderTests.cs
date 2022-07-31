using System;
using FluentAssertions;
using FluentCaching.Keys.Builders;
using FluentCaching.PolicyBuilders.Ttl;
using Moq;
using Xunit;

namespace FluentCaching.Tests.Unit.PolicyBuilders.Ttl
{
    public class TtlTypePolicyBuilderTests
    {
        private readonly Mock<IKeyBuilder> _keyBuilderMock;

        private readonly TtlTypePolicyBuilder _sut;

        public TtlTypePolicyBuilderTests()
        {
            _keyBuilderMock = new Mock<IKeyBuilder>();

            _sut = new TtlTypePolicyBuilder(_keyBuilderMock.Object);
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
