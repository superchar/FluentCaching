using System.Threading.Tasks;
using FluentCaching.Cache.Models;
using FluentCaching.Cache.Strategies.Store;
using FluentCaching.Tests.Unit.Models;
using Moq;
using Xunit;

namespace FluentCaching.Tests.Unit.Cache.Strategies.Store
{
    public class StoreStrategyTests : BaseCacheStrategyTests
    {
        private readonly StoreStrategy<User> _sut;

        public StoreStrategyTests()
        {
            _sut = new StoreStrategy<User>(CacheConfigurationMock.Object);
        }

        [Fact]
        public void CacheAsync_WhenCalled_CallsKeyBuilder()
        {
            _sut.StoreAsync(TestUser);

            KeyBuilderMock
                .Verify(_ => _.BuildFromCachedObject(TestUser), Times.Once);
        }

        [Fact]
        public async Task CacheAsync_WhenCalled_CallsCacheImplementation()
        {
            const string key = "key";
            KeyBuilderMock
                .Setup(_ => _.BuildFromCachedObject(TestUser))
                .Returns(key);
            
            await _sut.StoreAsync(TestUser);

            TypeCacheImplementationMock
                .Verify(_ => _.CacheAsync(
                        key,
                        TestUser,
                        It.IsAny<CacheOptions>()),
                    Times.Once);
        }
    }
}