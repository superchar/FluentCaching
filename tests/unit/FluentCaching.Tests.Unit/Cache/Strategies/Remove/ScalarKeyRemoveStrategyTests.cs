using System.Threading.Tasks;
using FluentCaching.Cache.Models;
using FluentCaching.Cache.Strategies.Remove;
using FluentCaching.Tests.Unit.TestModels;
using Moq;
using Xunit;

namespace FluentCaching.Tests.Unit.Cache.Strategies.Remove;

public class ScalarKeyRemoveStrategyTests : BaseCacheStrategyTests
{
    private static readonly CacheSource<User> StringKeySource =
        CacheSource<User>.Create("key part");

    private readonly ScalarKeyRemoveStrategy<User> _sut;

    public ScalarKeyRemoveStrategyTests()
    {
        _sut = new ScalarKeyRemoveStrategy<User>(CacheConfigurationMock.Object);
    }
    
    [Fact]
    public async Task RemoveAsync_WhenCalled_CallsKeyBuilder()
    {
        await _sut.RemoveAsync(StringKeySource);

        KeyBuilderMock
            .Verify(_ => _.BuildFromScalarKey(StringKeySource.Key), Times.Once);
    }

    [Fact]
    public async Task RemoveAsync_WhenCalled_CallsCacheImplementation()
    {
        const string key = "key";
        KeyBuilderMock
            .Setup(_ => _.BuildFromScalarKey(StringKeySource.Key))
            .Returns(key);
            
        await _sut.RemoveAsync(StringKeySource);

        TypeCacheImplementationMock
            .Verify(_ => _.RemoveAsync(key), Times.Once);
    }
}