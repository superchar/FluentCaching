using System.Threading.Tasks;
using FluentCaching.Cache.Models;
using FluentCaching.Cache.Strategies.Remove;
using FluentCaching.Tests.Unit.Models;
using Moq;
using Xunit;

namespace FluentCaching.Tests.Unit.Cache.Strategies.Remove
{
    public class ObjectKeyRemoveStrategyTests : BaseCacheStrategyTests
    {
        private static readonly CacheSource<User> ObjectKeySource = new(new object());
        
        private readonly ObjectKeyRemoveStrategy<User> _sut;

        public ObjectKeyRemoveStrategyTests()
        {
            _sut = new ObjectKeyRemoveStrategy<User>(CacheConfigurationMock.Object);
        }
        
        [Fact]
        public async Task RemoveAsync_WhenCalled_CallsKeyBuilder()
        {
            await _sut.RemoveAsync(ObjectKeySource);

            KeyBuilderMock
                .Verify(_ => _.BuildFromObjectKey(ObjectKeySource.ObjectKey), Times.Once);
        }

        [Fact]
        public async Task RemoveAsync_WhenCalled_CallsCacheImplementation()
        {
            const string key = "key";
            KeyBuilderMock
                .Setup(_ => _.BuildFromObjectKey(ObjectKeySource.ObjectKey))
                .Returns(key);
            
            await _sut.RemoveAsync(ObjectKeySource);

            TypeCacheImplementationMock
                .Verify(_ => _.RemoveAsync(key), Times.Once);
        }
    }
}