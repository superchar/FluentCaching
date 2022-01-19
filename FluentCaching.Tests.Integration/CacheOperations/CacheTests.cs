using System;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;
using FluentCaching.Exceptions;
using FluentCaching.Tests.Integration.Models;
using Moq;
using FluentCaching.Cache.Models;

namespace FluentCaching.Tests.Integration.CacheOperations
{
    public class CacheTests : CacheOperationBaseTest
    {
        [Fact]
        public async Task CacheAsync_CalledWithKeyAndValue_CallsStoreInImplementation()
        {
            await Cache.CacheAsync(User.Test);

            CacheImplementationMock
                   .Verify(i => i.CacheAsync(KEY, User.Test, It.IsAny<CacheOptions>()), Times.Once);
        }

        [Fact]
        public async Task RetrieveAsync_MissingConfiguration_ThrowsException()
        {
            Func<Task> cacheAsync = async () => await Cache.CacheAsync(Order.Test);

            await cacheAsync.Should().ThrowAsync<ConfigurationNotFoundException>();
            CacheImplementationMock
                .Verify(i => i.CacheAsync(It.IsAny<string>(), It.IsAny<User>(), It.IsAny<CacheOptions>()), Times.Never);
        }
    }
}
