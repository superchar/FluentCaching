using FluentAssertions;
using FluentCaching.Cache;
using FluentCaching.Cache.Builders;
using FluentCaching.Configuration;
using FluentCaching.PolicyBuilders;
using FluentCaching.PolicyBuilders.Keys;
using FluentCaching.PolicyBuilders.Ttl;
using Moq;
using System;
using Xunit;

namespace FluentCaching.Tests.Unit.Cache.Builders
{
    public class CacheBuilderTests
    {
        private readonly Mock<ICacheConfiguration> _cacheConfigurationMock;

        private readonly CacheBuilder _sut;

        public CacheBuilderTests()
        {
            _cacheConfigurationMock = new Mock<ICacheConfiguration>();

            _sut = new CacheBuilder(_cacheConfigurationMock.Object);
        }

        [Fact]
        public void Build_HappyPath_ReturnsCacheObject()
        {
            var result = _sut.Build();

            result.Should().NotBeNull();
        }

        [Fact]
        public void For_SpecificCacheHappyPath_CallsCorrespondingMethodInConfiguration() 
        {
            var factoryFuncMock = new Mock<Func<CachingKeyBuilder<string>, CacheImplementationBuilder>>();

            _sut.For(factoryFuncMock.Object);

            _cacheConfigurationMock
                .Verify(c => c.For(It.IsAny<Func<CachingKeyBuilder<string>, CacheImplementationBuilder>>()), Times.Once);
        }

        [Fact]
        public void For_GenericCacheHappyPath_CallsCorrespondingMethodInConfiguration() 
        {
            var factoryFuncMock = new Mock<Func<CachingKeyBuilder<string>, AndBuilder<CacheImplementationBuilder>>>();

            _sut.For(factoryFuncMock.Object);

            _cacheConfigurationMock
                .Verify(c => c.For(It.IsAny<Func<CachingKeyBuilder<string>, AndBuilder<CacheImplementationBuilder>>>()), Times.Once);
        }

        [Fact]
        public void SetGenericCache_HappyPath_CallsCorrespondingMethodInConfiguration() 
        {
            var genericCacheImplementationMock = new Mock<ICacheImplementation>();

            _sut.SetGenericCache(genericCacheImplementationMock.Object);

            _cacheConfigurationMock
                .Verify(c => c.SetGenericCache(It.IsAny<ICacheImplementation>()), Times.Once);
        }
    }
}
