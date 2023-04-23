using System;
using FluentAssertions;
using FluentCaching.Cache;
using FluentCaching.Cache.Models;
using FluentCaching.Configuration.PolicyBuilders;
using FluentCaching.Keys.Builders;
using Moq;
using Xunit;

namespace FluentCaching.Tests.Unit.Configuration.PolicyBuilders;

public class CacheImplementationPolicyBuilderTests
{
    private readonly CacheOptions _cacheOptions;

    private readonly CacheImplementationPolicyBuilder _sut;

    public CacheImplementationPolicyBuilderTests()
    {
        _cacheOptions = new CacheOptions(new Mock<IKeyBuilder>().Object);

        _sut = new CacheImplementationPolicyBuilder(_cacheOptions);
    }

    [Fact]
    public void StoreIn_CacheImplementationIsNull_ThrowsException()
    {
        _sut.Invoking(s => s.StoreIn(null)).Should()
            .Throw<ArgumentNullException>().WithMessage("Cache implementation cannot be null (Parameter 'cacheImplementation')");
    }

    [Fact]
    public void StoreIn_CacheImplementationIsNotNull_SetsCacheImplementation()
    {
        var cacheImplementationMock = new Mock<ICacheImplementation>();

        _sut.StoreIn(cacheImplementationMock.Object);

        _cacheOptions.CacheImplementation.Should().NotBeNull();
    }
}