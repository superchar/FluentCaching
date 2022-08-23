using System.Threading.Tasks;
using FluentCaching.Cache.Models;
using FluentCaching.Cache.Strategies.Remove;
using FluentCaching.Tests.Unit.Models;
using Moq;
using Xunit;

namespace FluentCaching.Tests.Unit.Cache.Strategies.Remove;

public class StaticKeyRemoveStrategyTests : BaseCacheStrategyTests
{
    private readonly StaticKeyRemoveStrategy<User> _sut;

    public StaticKeyRemoveStrategyTests()
    {
        _sut = new StaticKeyRemoveStrategy<User>(CacheConfigurationMock.Object);
    }
    
    [Fact]
    public async Task RemoveAsync_WhenCalled_CallsKeyBuilder()
    {
        await _sut.RemoveAsync(CacheSource<User>.Static);

        KeyBuilderMock
            .Verify(_ => _.BuildFromStaticKey(), Times.Once);
    }

    [Fact]
    public async Task RemoveAsync_WhenCalled_CallsCacheImplementation()
    {
        const string key = "key";
        KeyBuilderMock
            .Setup(_ => _.BuildFromStaticKey())
            .Returns(key);
            
        await _sut.RemoveAsync(CacheSource<User>.Static);

        TypeCacheImplementationMock
            .Verify(_ => _.RemoveAsync(key), Times.Once);
    }
}