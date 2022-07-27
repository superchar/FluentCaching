using System.Threading.Tasks;
using FluentCaching.Cache.Models;
using FluentCaching.Cache.Strategies.Remove;
using FluentCaching.Tests.Unit.Models;
using Moq;
using Xunit;

namespace FluentCaching.Tests.Unit.Cache.Strategies.Remove;

public class StringKeyRemoveStrategyTests : BaseCacheStrategyTests
{
    private static readonly CacheSource<User> StringKeySource = new("key part");

    private readonly StringKeyRemoveStrategy<User> _sut;

    public StringKeyRemoveStrategyTests()
    {
        _sut = new StringKeyRemoveStrategy<User>(CacheConfigurationMock.Object);
    }
    
    [Fact]
    public async Task RemoveAsync_WhenCalled_CallsKeyBuilder()
    {
        await _sut.RemoveAsync(StringKeySource);

        KeyBuilderMock
            .Verify(_ => _.BuildFromStringKey(StringKeySource.StringKey), Times.Once);
    }

    [Fact]
    public async Task RemoveAsync_WhenCalled_CallsCacheImplementation()
    {
        const string key = "key";
        KeyBuilderMock
            .Setup(_ => _.BuildFromStringKey(StringKeySource.StringKey))
            .Returns(key);
            
        await _sut.RemoveAsync(StringKeySource);

        TypeCacheImplementationMock
            .Verify(_ => _.RemoveAsync(key), Times.Once);
    }
}