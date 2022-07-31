using System;
using FluentAssertions;
using FluentCaching.Cache;
using FluentCaching.Cache.Models;
using FluentCaching.PolicyBuilders;
using Moq;
using Xunit;

namespace FluentCaching.Tests.Unit.PolicyBuilders
{
    public class CacheImplementationPolicyBuilderTests
    {
        private readonly CacheOptions _cacheOptions;

        private readonly CacheImplementationPolicyBuilder _sut;

        public CacheImplementationPolicyBuilderTests()
        {
            _cacheOptions = new CacheOptions();

            _sut = new CacheImplementationPolicyBuilder(_cacheOptions);
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
