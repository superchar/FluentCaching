using FluentAssertions;
using FluentCaching.Cache;
using FluentCaching.Cache.Models;
using FluentCaching.Configuration;
using FluentCaching.Configuration.Exceptions;
using FluentCaching.Tests.Unit.Models;
using Moq;
using Xunit;

namespace FluentCaching.Tests.Unit.Cache.Strategies
{
    public class TestBaseCacheStrategyWithStrategyTestsTests : BaseCacheStrategyTests
    {
        private TestBaseCacheStrategyWithConfiguration _sut;

        public TestBaseCacheStrategyWithStrategyTestsTests()
        {
            _sut = new TestBaseCacheStrategyWithConfiguration(CacheConfigurationMock.Object);
        }

        [Fact]
        public void GetConfigurationItem_WhenCalled_CallsCacheConfiguration()
        {
            _sut.GetConfigurationItem<User>();
            
            CacheConfigurationMock
                .Verify(_ => _.GetItem<User>(), Times.Once);
        }
        
        [Fact]
        public void GetConfigurationItem_ConfigurationItemIsNull_ThrowsConfigurationNotFoundException()
        {
            CacheConfigurationMock
                .Setup(_ => _.GetItem<User>())
                .Returns((CacheConfigurationItem)null);

            _sut.Invoking(_ => _.GetConfigurationItem<User>())
                .Should()
                .Throw<ConfigurationNotFoundException>();
        }

        [Fact]
        public void GetCacheImplementation_ImplementationConfiguredForType_ReturnsImplementationForType()
        {
            var typeImplementation = TypeCacheImplementationMock.Object;
            var globalImplementation = GlobalCacheImplementationMock.Object;

            var implementation = _sut.GetCacheImplementation<User>(ConfigurationItemMock.Object);

            implementation.Should().BeSameAs(typeImplementation).And
                .NotBeSameAs(globalImplementation);
        }
        
        [Fact]
        public void GetCacheImplementation_ImplementationIsNotConfiguredForType_ReturnsGlobalImplementation()
        {
            SetupEmptyCacheOptions();
            
            var implementation = _sut.GetCacheImplementation<User>(ConfigurationItemMock.Object);

            implementation.Should().BeSameAs(GlobalCacheImplementationMock.Object);
        }
        
        [Fact]
        public void GetCacheImplementation_GlobalAndTypeImplementationsAreNotConfigured_ThrowsCacheImplementationNotFoundException()
        {
            SetupEmptyCacheOptions();
            CacheConfigurationMock
                .SetupGet(_ => _.Current)
                .Returns((ICacheImplementation)null);
            
            _sut.Invoking(_ => _.GetCacheImplementation<User>(ConfigurationItemMock.Object))
                .Should().Throw<CacheImplementationNotFoundException>();
        }

        private void SetupEmptyCacheOptions()
        {
            ConfigurationItemMock
                .SetupGet(_ => _.Options)
                .Returns(new CacheOptions());
        }
    }
}