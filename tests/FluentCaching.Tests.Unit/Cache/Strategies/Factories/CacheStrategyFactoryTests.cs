using System.Collections.Generic;
using FluentAssertions;
using FluentCaching.Cache.Models;
using FluentCaching.Cache.Strategies.Factories;
using FluentCaching.Cache.Strategies.Remove;
using FluentCaching.Cache.Strategies.Retrieve;
using FluentCaching.Cache.Strategies.RetrieveOrStore;
using FluentCaching.Cache.Strategies.Store;
using FluentCaching.Configuration;
using FluentCaching.Tests.Unit.Models;
using Moq;
using Xunit;

namespace FluentCaching.Tests.Unit.Cache.Strategies.Factories;

public class CacheStrategyFactoryTests
{
    private static readonly CacheSource<User> CacheSourceWithObjectKey = new(new object());
    private static readonly CacheSource<User> CacheSourceWithStringKey = new("key");

    private readonly CacheStrategyFactory _sut;

    public CacheStrategyFactoryTests()
    {
        var cacheConfigurationMock = new Mock<ICacheConfiguration>();

        _sut = new CacheStrategyFactory(cacheConfigurationMock.Object);
    }

    public static IEnumerable<object[]> CacheSources
    {
        get
        {
            yield return new object[]{ CacheSourceWithObjectKey };
            yield return new object[]{ CacheSourceWithStringKey };
        }
    }

    [Fact]
    public void CreateStoreStrategy_WhenCalled_ReturnsStoreStrategy()
    {
        var result = _sut.CreateStoreStrategy<User>();

        result.Should().BeOfType<StoreStrategy<User>>();
    }

    [Fact]
    public void CreateRetrieveStrategy_CacheSourceIsNull_ReturnsStaticKeyRetrieveStrategy()
    {
        var result = _sut.CreateRetrieveStrategy(CacheSource<User>.Null);

        result.Should().BeOfType<StaticKeyRetrieveStrategy<User>>();
    }
    
    [Fact]
    public void CreateRetrieveStrategy_CacheSourceWithStringKey_ReturnsStringKeyRetrieveStrategy()
    {
        var result = _sut.CreateRetrieveStrategy(CacheSourceWithStringKey);

        result.Should().BeOfType<StringKeyRetrieveStrategy<User>>();
    }
    
    [Fact]
    public void CreateRetrieveStrategy_CacheSourceWithObjectKey_ReturnsObjectKeyRetrieveStrategy()
    {
        var result = _sut.CreateRetrieveStrategy(CacheSourceWithObjectKey);

        result.Should().BeOfType<ObjectKeyRetrieveStrategy<User>>();
    }
    
    [Fact]
    public void CreateRemoveStrategy_CacheSourceWithStringKey_ReturnsStringKeyRemoveStrategy()
    {
        var result = _sut.CreateRemoveStrategy(CacheSourceWithStringKey);

        result.Should().BeOfType<StringKeyRemoveStrategy<User>>();
    }
    
    [Fact]
    public void CreateRemoveStrategy_CacheSourceWithObjectKey_ReturnsStringKeyRemoveStrategy()
    {
        var result = _sut.CreateRemoveStrategy(CacheSourceWithObjectKey);

        result.Should().BeOfType<ObjectKeyRemoveStrategy<User>>();
    }
    
    [Theory]
    [MemberData(nameof(CacheSources))]
    public void CreateRetrieveOrStoreStrategy_WhenCalled_ReturnsCacheSourceRetrieveOrStoreStrategy(CacheSource<User> cacheSource)
    {
        var result = _sut.CreateRetrieveOrStoreStrategy(cacheSource);

        result.Should().BeOfType<CacheSourceRetrieveOrStoreStrategy<User>>();
    }
}