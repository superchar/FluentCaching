using System;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;
using FluentCaching.Exceptions;
using FluentCaching.Tests.Integration.Models;
using Moq;

namespace FluentCaching.Tests.Integration.CacheOperations
{
    public class RemoveTests : CacheOperationBaseTest
    {
        [Fact]
        public async Task RemoveAsync_MissingConfiguration_ThrowsException()
        {

            Func<Task<Order>> retrieveAsync = async () => await Cache.RetrieveAsync<Order>(new { Id = 1, LastName = "Test" });

            await retrieveAsync.Should().ThrowAsync<ConfigurationNotFoundException>();
        }

        [Fact]
        public async Task RemoveAsync_CalledWithKey_CallsRemoveInImplementation()
        {
            await Cache.RemoveAsync<User>(KEY);

            CacheImplementationMock
                .Verify(i => i.RemoveAsync(KEY), Times.Once);
        }
    }
}
