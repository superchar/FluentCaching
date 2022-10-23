using System.Threading.Tasks;
using FluentAssertions;
using FluentCaching.Configuration.Exceptions;
using FluentCaching.Tests.Integration.Models;
using Xunit;

namespace FluentCaching.Tests.Integration.Cache
{
    public class RetrieveTests : CacheOperationBaseTest
    {
        [Fact]
        public async Task RetrieveAsync_ConfigurationExists_RetrievesObjectFromCache()
        {
            var user = new User();
            CacheImplementation.Dictionary[Key] = user;

            var result = await Cache.RetrieveAsync<User>(Key);

            result.Should().Be(user);
        }

        [Fact]
        public async Task RetrieveAsync_ConfigurationDoesNotExist_ThrowsException()
        {
            await Cache.Invoking(c => c.RetrieveAsync<Order>(new { Id = 1, LastName = "Test" }))
                .Should().ThrowAsync<ConfigurationNotFoundException>();
        }
    }
}
