using System.Collections.Generic;
using FluentAssertions;
using FluentCaching.Cache.Models;
using FluentCaching.Cache.Strategies.Factories;
using FluentCaching.Cache.Strategies.Remove;
using FluentCaching.Cache.Strategies.Retrieve;
using FluentCaching.Cache.Strategies.Store;
using FluentCaching.Configuration;
using FluentCaching.Tests.Unit.Models;
using Moq;
using Xunit;

namespace FluentCaching.Tests.Unit.Cache.Strategies.Factories;

public class CacheStrategyFactoryTests
{
    private static readonly CacheSource<User> ComplexKeyCacheSource = 
        CacheSource<User>.CreateComplex(new object());
    private static readonly CacheSource<User> ScalarKeyCacheSource = 
        CacheSource<User>.CreateScalar("key");

    private readonly CacheStrategyFactory _sut;

    public CacheStrategyFactoryTests()
    {
        var cacheConfigurationMock = new Mock<ICacheConfiguration>();

        _sut = new CacheStrategyFactory(cacheConfigurationMock.Object);
    }

    [Fact]
    public void CreateStoreStrategy_WhenCalled_ReturnsStoreStrategy()
    {
        var result = _sut.CreateStoreStrategy<User>();

        result.Should().BeOfType<StoreStrategy<User>>();
    }

    [Fact]
    public void CreateRetrieveStrategy_CacheSourceIsStatic_ReturnsStaticKeyRetrieveStrategy()
    {
        var result = _sut.CreateRetrieveStrategy(CacheSource<User>.Static);

        result.Should().BeOfType<StaticKeyRetrieveStrategy<User>>();
    }
    
    [Fact]
    public void CreateRetrieveStrategy_CacheSourceIsScalar_ReturnsScalarKeyRetrieveStrategy()
    {
        var result = _sut.CreateRetrieveStrategy(ScalarKeyCacheSource);

        result.Should().BeOfType<ScalarKeyRetrieveStrategy<User>>();
    }
    
    [Fact]
    public void CreateRetrieveStrategy_CacheSourceIsComplex_ReturnsComplexKeyRetrieveStrategy()
    {
        var result = _sut.CreateRetrieveStrategy(ComplexKeyCacheSource);

        result.Should().BeOfType<ComplexKeyRetrieveStrategy<User>>();
    }
    
    [Fact]
    public void CreateRemoveStrategy_CacheSourceIsStatic_ReturnsScalarKeyRemoveStrategy()
    {
        var result = _sut.CreateRemoveStrategy(CacheSource<User>.Static);

        result.Should().BeOfType<StaticKeyRemoveStrategy<User>>();
    }
    
    [Fact]
    public void CreateRemoveStrategy_CacheSourceIsScalar_ReturnsScalarKeyRemoveStrategy()
    {
        var result = _sut.CreateRemoveStrategy(ScalarKeyCacheSource);

        result.Should().BeOfType<ScalarKeyRemoveStrategy<User>>();
    }
    
    [Fact]
    public void CreateRemoveStrategy_CacheSourceIsComplex_ReturnsComplexKeyRemoveStrategy()
    {
        var result = _sut.CreateRemoveStrategy(ComplexKeyCacheSource);

        result.Should().BeOfType<ComplexKeyRemoveStrategy<User>>();
    }
}