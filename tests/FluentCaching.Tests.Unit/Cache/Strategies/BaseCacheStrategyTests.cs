using FluentCaching.Cache;
using FluentCaching.Cache.Models;
using FluentCaching.Configuration;
using FluentCaching.Keys.Builders;
using FluentCaching.Tests.Unit.Models;
using Moq;

namespace FluentCaching.Tests.Unit.Cache.Strategies
{
    public abstract class BaseCacheStrategyTests
    {
        private protected User TestUser { get; }
        
        private protected Mock<ICacheConfiguration> CacheConfigurationMock { get; }
        private protected Mock<ICacheConfigurationItem<User>> ConfigurationItemMock { get; }
        private protected Mock<ICacheImplementation> GlobalCacheImplementationMock { get; }
        private protected Mock<ICacheImplementation> TypeCacheImplementationMock { get;  }
        private protected Mock<IKeyBuilder<User>> KeyBuilderMock { get; }

        protected BaseCacheStrategyTests()
        {
            TestUser = new User
            {
                Id = 42,
                Name = "User"
            };
            
            ConfigurationItemMock = new Mock<ICacheConfigurationItem<User>>();
            CacheConfigurationMock = new Mock<ICacheConfiguration>();
            GlobalCacheImplementationMock = new Mock<ICacheImplementation>();
            TypeCacheImplementationMock = new Mock<ICacheImplementation>();
            KeyBuilderMock = new Mock<IKeyBuilder<User>>();
            
            SetupCacheConfiguration();
        }

        private void SetupCacheConfiguration()
        {
            var options = new CacheOptions
            {
                CacheImplementation = TypeCacheImplementationMock.Object,
                KeyBuilder = KeyBuilderMock.Object
            };
            
            ConfigurationItemMock
                .SetupGet(_ => _.Options)
                .Returns(options);
            ConfigurationItemMock
                .SetupGet(_ => _.KeyBuilder)
                .Returns(KeyBuilderMock.Object);
            CacheConfigurationMock
                .Setup(_ => _.GetItem<User>())
                .Returns(ConfigurationItemMock.Object);
            CacheConfigurationMock
                .SetupGet(_ => _.Current)
                .Returns(GlobalCacheImplementationMock.Object);
        }
    }
}