using System;
using FluentAssertions;
using Moq;
using Xunit;
using FluentCaching.Cache;
using FluentCaching.Cache.Models;
using FluentCaching.Configuration;
using FluentCaching.PolicyBuilders;
using FluentCaching.PolicyBuilders.Keys;
using FluentCaching.PolicyBuilders.Ttl;
using FluentCaching.Tests.Unit.Models;

namespace FluentCaching.Tests.Unit.Configuration
{
    public class CacheConfigurationTests
    {
        private readonly CacheConfiguration _sut = new CacheConfiguration();

        [Fact]
        public void SetGenericCache_CacheImplementationIsNull_ThrowsException()
        {
            _sut.Invoking(s => s.SetGenericCache(null)).Should()
                .Throw<ArgumentNullException>().WithMessage("Cache implementation cannot be null (Parameter 'cacheImplementation')");
        }

        [Fact]
        public void SetGenericCache_CacheImplementationIsNotNull_DoesNotThrowException()
        {
            var cacheImplementationMock = new Mock<ICacheImplementation>();

            _sut.Invoking(s => s.SetGenericCache(cacheImplementationMock.Object))
                .Should().NotThrow();
        }

        [Fact]
        public void For_GenericCache_CallsFactoryWithCachingKeyBuilder()
        {
            var factoryMock = new Mock<Func<CachingKeyBuilder<User>, AndBuilder<CacheImplementationBuilder>>>();
            factoryMock
                .Setup(f => f(It.IsAny<CachingKeyBuilder<User>>()))
                .Returns(new AndBuilder<CacheImplementationBuilder>(new CacheImplementationBuilder(new CacheOptions())));

            _sut.For(factoryMock.Object);

            factoryMock
                .Verify(f => f(It.IsAny<CachingKeyBuilder<User>>()), Times.Once);
        }

        [Fact]
        public void For_SpecificCache_CallsFactoryWithCachingKeyBuilder()
        {
            var factoryMock = new Mock<Func<CachingKeyBuilder<User>, CacheImplementationBuilder>>();
            factoryMock
                .Setup(f => f(It.IsAny<CachingKeyBuilder<User>>()))
                .Returns(new CacheImplementationBuilder(new CacheOptions()));

            _sut.For(factoryMock.Object);

            factoryMock
                .Verify(f => f(It.IsAny<CachingKeyBuilder<User>>()), Times.Once);
        }

        [Fact]
        public void GetItem_ConfigurationDoesNotExist_ReturnsNull()
        {
            var result = _sut.GetItem<User>();

            result.Should().BeNull();
        }

        [Fact]
        public void GetItem_ConfigurationExists_ReturnsConfiguration()
        {
            var factoryMock = new Mock<Func<CachingKeyBuilder<User>, AndBuilder<CacheImplementationBuilder>>> ();
            factoryMock
                .Setup(f => f(It.IsAny<CachingKeyBuilder<User>>()))
                .Returns(new AndBuilder<CacheImplementationBuilder>(new CacheImplementationBuilder(new CacheOptions())));
            _sut.For(factoryMock.Object);

            var result = _sut.GetItem<User>();

            result.Should().NotBeNull();
            factoryMock
                .Verify(f => f(It.IsAny<CachingKeyBuilder<User>>()), Times.Once);
        }
    }
}
