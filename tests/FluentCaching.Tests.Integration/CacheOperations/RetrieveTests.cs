using System;
using System.Threading.Tasks;
using FluentAssertions;
using FluentCaching.Configuration.Exceptions;
using FluentCaching.Tests.Integration.Models;
using Moq;
using Xunit;

namespace FluentCaching.Tests.Integration.CacheOperations
{
    public class RetrieveTests : CacheOperationBaseTest
    {
        [Fact]
        public async Task RetrieveAsync_CalledWithKey_CallsRetrieveInImplementation()
        {
            await Cache.RetrieveAsync<User>(Key);

            CacheImplementationMock
                   .Verify(i => i.RetrieveAsync<User>(Key), Times.Once);
        }

        [Fact]
        public async Task RetrieveAsync_MissingConfiguration_ThrowsException()
        {
            Func<Task<Order>> retrieveAsync = async () => await Cache.RetrieveAsync<Order>(new { Id = 1, LastName = "Test" });

            await retrieveAsync.Should().ThrowAsync<ConfigurationNotFoundException>();
            CacheImplementationMock
                .Verify(i => i.RetrieveAsync<Order>(It.IsAny<string>()), Times.Never);
        }
    }
}
