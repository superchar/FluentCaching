using System;
using System.Threading.Tasks;
using FluentAssertions;
using FluentCaching.Configuration.Exceptions;
using FluentCaching.Tests.Integration.Fakes;
using FluentCaching.Tests.Integration.Models;
using Moq;
using Xunit;

namespace FluentCaching.Tests.Integration.CacheOperations
{
    public class RetrieveOrStoreTests : CacheOperationBaseTest
    {
        [Fact]
        public async Task RetrieveOrStoreAsync_MissingConfiguration_ThrowsException()
        {
            var retrieveOrStoreAsync = () => Cache.RetrieveAsync<Order>(new { Id = 1, LastName = "Test" });

            await retrieveOrStoreAsync.Should().ThrowAsync<ConfigurationNotFoundException>();
        }

        [Fact]
        public async Task RetrieveOrStoreAsync_ValueIsInCache_ReturnsCachedValue()
        {
            await Cache.CacheAsync(User.Test);
            var entityFetcherMock = new Mock<Func<string, Task<User>>>();
            CacheImplementationMock
                .Setup(i => i.RetrieveAsync<User>(Key))
                .ReturnsAsync(User.Test);

            var result = await Cache.RetrieveOrStoreAsync(Key, entityFetcherMock.Object);

            result.Should().Be(User.Test);
            entityFetcherMock.Verify(f => f(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task RetrieveOrStoreAsync_ValueIsNotInCache_CachesValueUsingEntityFetcher()
        {
            var entityFetcherMock = new Mock<Func<string, Task<User>>>();
            entityFetcherMock.Setup(f => f(Key)).ReturnsAsync(User.Test);

            var result = await Cache.RetrieveOrStoreAsync(Key, entityFetcherMock.Object);

            result.Should().Be(User.Test);
            entityFetcherMock.Verify(f => f(Key), Times.Once);
        }

        [Fact]
        public async Task RetrieveOrStoreAsync_MultipleCallsForTheSameKey_CallsEntityFetcherOnlyOnce()
        {
            CacheBuilder
                .SetGenericCache(new DictionaryCacheImplementation());

            var entityFetcherMock = new Mock<Func<string, Task<User>>>();
            entityFetcherMock
                .Setup(f => f(Key))
                .ReturnsAsync(User.Test);

            await Cache.RetrieveOrStoreAsync(Key, entityFetcherMock.Object);
            
            await Cache.RetrieveOrStoreAsync(Key, entityFetcherMock.Object);
            entityFetcherMock.Verify(f => f(Key), Times.Once);
        }
    }
}
