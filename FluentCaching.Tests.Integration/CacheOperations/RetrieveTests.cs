using System;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;
using FluentCaching.Exceptions;
using FluentCaching.Tests.Integration.Models;
using Moq;

namespace FluentCaching.Tests.Integration.CacheOperations
{
    public class RetrieveTests : CacheOperationBaseTest
    {
        [Fact]
        public async Task RetrieveAsync_CalledWithKey_CallsRetrieveInImplementation()
        {
            var result = await Cache.RetrieveAsync<User>(KEY);

            CacheImplementationMock
                   .Verify(i => i.RetrieveAsync<User>(KEY), Times.Once);
        }

        [Fact]
        public void RetrieveAsync_MissingConfiguration_ThrowsException()
        {
            Func<Task<Order>> retrieveAsync = async () => await Cache.RetrieveAsync<Order>(new { Id = 1, LastName = "Test" });

            retrieveAsync.Should().Throw<ConfigurationNotFoundException>();
            CacheImplementationMock
                .Verify(i => i.RetrieveAsync<Order>(It.IsAny<string>()), Times.Never);
        }
    }
}
