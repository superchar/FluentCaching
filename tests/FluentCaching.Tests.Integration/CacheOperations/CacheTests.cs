using System;
using System.Threading.Tasks;
using FluentAssertions;
using FluentCaching.Cache.Models;
using FluentCaching.Configuration.Exceptions;
using FluentCaching.Tests.Integration.Models;
using Moq;
using Xunit;

namespace FluentCaching.Tests.Integration.CacheOperations
{
    public class CacheTests : CacheOperationBaseTest
    {
        [Fact]
        public async Task CacheAsync_CalledWithKeyAndValue_CallsStoreInImplementation()
        {
            await Cache.CacheAsync(User.Test);

            CacheImplementationMock
                   .Verify(i => i.CacheAsync(Key, User.Test, It.IsAny<CacheOptions>()), Times.Once);
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
