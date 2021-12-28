using System;
using Moq;
using Xunit;
using FluentAssertions;
using FluentCaching.Cache;
using FluentCaching.Cache.Models;
using FluentCaching.PolicyBuilders;

namespace FluentCaching.Tests.Unit.PolicyBuilders
{
    public class CacheImplementationBuilderTests
    {
        private readonly CacheOptions _cacheOptions;

        private readonly CacheImplementationBuilder _sut;

        public CacheImplementationBuilderTests()
        {
            _cacheOptions = new CacheOptions();

            _sut = new CacheImplementationBuilder(_cacheOptions);
        }

        [Fact]
        public void WithCacheImplementation_CacheImplementationIsNull_ThrowsException()
        {
            _sut.Invoking(s => s.WithCacheImplementation(null)).Should()
                .Throw<ArgumentNullException>().WithMessage("Cache implementation cannot be null (Parameter 'cacheImplementation')");
        }

        [Fact]
        public void WithCacheImplementation_CacheImplementationIsNotNull_SetsCacheImplementation()
        {
            var cacheImplementationMock = new Mock<ICacheImplementation>();

            _sut.WithCacheImplementation(cacheImplementationMock.Object);

            _cacheOptions.CacheImplementation.Should().NotBeNull();
        }
    }
}
