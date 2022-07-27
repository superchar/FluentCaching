using System;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;
using FluentCaching.Keys;
using FluentCaching.Tests.Integration.Models;
using Moq;
using System.Linq;
using FluentCaching.Configuration.Exceptions;
using FluentCaching.Tests.Integration.Fakes;

namespace FluentCaching.Tests.Integration.CacheOperations
{
    public class RetrieveOrStoreTests : CacheOperationBaseTest
    {
        [Fact]
        public async Task RetrieveOrStoreAsync_MissingConfiguration_ThrowsException()
        {
            Func<Task<Order>> retrieveOrStoreAsync = () => Cache.RetrieveAsync<Order>(new { Id = 1, LastName = "Test" });

            await retrieveOrStoreAsync.Should().ThrowAsync<ConfigurationNotFoundException>();
        }

        [Fact]
        public async Task RetrieveOrStoreAsync_ValueIsInCache_ReturnsCachedValue()
        {
            await Cache.CacheAsync(User.Test);
            var entityFetcherMock = new Mock<Func<string, Task<User>>>();
            CacheImplementationMock
                .Setup(i => i.RetrieveAsync<User>(KEY))
                .ReturnsAsync(User.Test);

            var result = await Cache.RetrieveOrStoreAsync(KEY, entityFetcherMock.Object);

            result.Should().Be(User.Test);
            entityFetcherMock.Verify(f => f(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task RetrieveOrStoreAsync_ValueIsNotInCache_CachesValueUsingEntityFetcher()
        {
            var entityFetcherMock = new Mock<Func<string, Task<User>>>();
            entityFetcherMock.Setup(f => f(KEY)).ReturnsAsync(User.Test);

            var result = await Cache.RetrieveOrStoreAsync(KEY, entityFetcherMock.Object);

            result.Should().Be(User.Test);
            entityFetcherMock.Verify(f => f(KEY), Times.Once);
        }

        [Fact]
        public async Task RetrieveOrStoreAsync_MultipleCallsForTheSameKey_CallsEntityFetcherOnlyOnce()
        {
            CacheBuilder
                .SetGenericCache(new DictionaryCacheImplementation());

            var entityFetcherMock = new Mock<Func<string, Task<User>>>();
            entityFetcherMock
                .Setup(f => f(KEY))
                .ReturnsAsync(User.Test);

            Cache.RetrieveOrStoreAsync(KEY, entityFetcherMock.Object);
            
            Cache.RetrieveOrStoreAsync(KEY, entityFetcherMock.Object);
            entityFetcherMock.Verify(f => f(KEY), Times.Once);
        }
    }
}
