﻿using System;
using FluentAssertions;
using FluentCaching.Cache;
using FluentCaching.Cache.Models;
using FluentCaching.Configuration;
using FluentCaching.Configuration.PolicyBuilders;
using FluentCaching.Configuration.PolicyBuilders.Keys;
using FluentCaching.Keys.Builders;
using FluentCaching.Keys.Builders.Factories;
using FluentCaching.Tests.Unit.TestModels;
using Moq;
using Xunit;

namespace FluentCaching.Tests.Unit.Configuration;

public class CacheConfigurationTests
{
    private readonly CacheConfiguration _sut = new (new Mock<IKeyBuilderFactory>().Object);

    [Fact]
    public void SetGenericCache_CacheImplementationIsNull_ThrowsException()
    {
        _sut.Invoking(s => s.SetGenericCache(null)).Should()
            .Throw<ArgumentNullException>().WithMessage("Cache implementation cannot be null (Parameter 'cacheImplementation')");
    }

    [Fact]
    public void SetGenericCache_CacheImplementationIsNotNull_DoesNotThrowException()
    {
        var cacheImplementationMock = new Mock<ICacheImplementation>();

        _sut.Invoking(s => s.SetGenericCache(cacheImplementationMock.Object))
            .Should().NotThrow();
    }

    [Fact]
    public void For_GenericCache_CallsFactoryWithCachingKeyBuilder()
    {
        var factoryMock = new Mock<Func<CachingKeyPolicyBuilder<User>, AndPolicyBuilder<CacheImplementationPolicyBuilder>>>();
        factoryMock
            .Setup(f => f(It.IsAny<CachingKeyPolicyBuilder<User>>()))
            .Returns(new AndPolicyBuilder<CacheImplementationPolicyBuilder>(CreateCachePolicyBuilder()));

        _sut.For(factoryMock.Object);

        factoryMock
            .Verify(f => f(It.IsAny<CachingKeyPolicyBuilder<User>>()), Times.Once);
    }

    [Fact]
    public void For_SpecificCache_CallsFactoryWithCachingKeyBuilder()
    {
        var factoryMock = new Mock<Func<CachingKeyPolicyBuilder<User>, CacheImplementationPolicyBuilder>>();
        factoryMock
            .Setup(f => f(It.IsAny<CachingKeyPolicyBuilder<User>>()))
            .Returns(CreateCachePolicyBuilder());

        _sut.For(factoryMock.Object);

        factoryMock
            .Verify(f => f(It.IsAny<CachingKeyPolicyBuilder<User>>()), Times.Once);
    }

    [Fact]
    public void GetItem_ConfigurationDoesNotExist_ReturnsNull()
    {
        var result = _sut.GetItem<User>(CacheConfiguration.DefaultPolicyName);

        result.Should().BeNull();
    }

    [Fact]
    public void GetItem_ConfigurationExists_ReturnsConfiguration()
    {
        var factoryMock = new Mock<Func<CachingKeyPolicyBuilder<User>, AndPolicyBuilder<CacheImplementationPolicyBuilder>>> ();
        factoryMock
            .Setup(f => f(It.IsAny<CachingKeyPolicyBuilder<User>>()))
            .Returns(new AndPolicyBuilder<CacheImplementationPolicyBuilder>(CreateCachePolicyBuilder()));
        _sut.For(factoryMock.Object);

        var result = _sut.GetItem<User>(CacheConfiguration.DefaultPolicyName);

        result.Should().NotBeNull();
        factoryMock
            .Verify(f => f(It.IsAny<CachingKeyPolicyBuilder<User>>()), Times.Once);
    }

    private static CacheImplementationPolicyBuilder CreateCachePolicyBuilder()
        => new (new CacheOptions(new Mock<IKeyBuilder>().Object));
}