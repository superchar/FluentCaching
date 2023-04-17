using System.Threading.Tasks;
using FluentCaching.Cache.Models;
using FluentCaching.Cache.Strategies.Remove;
using FluentCaching.Tests.Unit.TestModels;
using Moq;
using Xunit;

namespace FluentCaching.Tests.Unit.Cache.Strategies.Remove;

public class ComplexKeyRemoveStrategyTests : BaseCacheStrategyTests
{
    private static readonly CacheSource<User> ObjectKeySource 
        = CacheSource<User>.Create(new object());
        
    private readonly ComplexKeyRemoveStrategy<User> _sut;

    public ComplexKeyRemoveStrategyTests()
    {
        _sut = new ComplexKeyRemoveStrategy<User>(CacheConfigurationMock.Object);
    }
        
    [Fact]
    public async Task RemoveAsync_WhenCalled_CallsKeyBuilder()
    {
        await _sut.RemoveAsync(ObjectKeySource);

        KeyBuilderMock
            .Verify(_ => _.BuildFromComplexKey(ObjectKeySource.Key), Times.Once);
    }

    [Fact]
    public async Task RemoveAsync_WhenCalled_CallsCacheImplementation()
    {
        const string key = "key";
        KeyBuilderMock
            .Setup(_ => _.BuildFromComplexKey(ObjectKeySource.Key))
            .Returns(key);
            
        await _sut.RemoveAsync(ObjectKeySource);

        TypeCacheImplementationMock
            .Verify(_ => _.RemoveAsync(key), Times.Once);
    }
}