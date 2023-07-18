using System.Threading.Tasks;
using FluentCaching.Cache.Models;
using FluentCaching.Cache.Strategies.Retrieve;
using FluentCaching.Configuration;
using FluentCaching.Tests.Unit.TestModels;
using Moq;
using Xunit;

namespace FluentCaching.Tests.Unit.Cache.Strategies.Retrieve;

public class ComplexKeyRetrieveStrategyTests : BaseCacheStrategyTests
{
    private static readonly CacheSource<User> ComplexKeySource 
        = CacheSource<User>.Create(new object());
    
    private readonly ComplexKeyRetrieveStrategy<User> _sut;

    public ComplexKeyRetrieveStrategyTests()
    {
        _sut = new ComplexKeyRetrieveStrategy<User>(CacheConfigurationMock.Object);
    }
    
    [Fact]
    public async Task RetrieveAsync_WhenCalled_CallsKeyBuilder()
    {
        await _sut.RetrieveAsync(ComplexKeySource, CacheConfiguration.DefaultPolicyName);

        KeyBuilderMock
            .Verify(_ => _.BuildFromComplexKey(ComplexKeySource.Key), Times.Once);
    }

    [Fact]
    public async Task RetrieveAsync_WhenCalled_CallsCacheImplementation()
    {
        const string key = "key";
        KeyBuilderMock
            .Setup(_ => _.BuildFromComplexKey(ComplexKeySource.Key))
            .Returns(key);
            
        await _sut.RetrieveAsync(ComplexKeySource, CacheConfiguration.DefaultPolicyName);

        TypeCacheImplementationMock
            .Verify(_ => _.RetrieveAsync<User>(key), Times.Once);
    }
}