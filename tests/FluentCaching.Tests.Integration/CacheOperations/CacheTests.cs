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
        public async Task CacheAsync_ConfigurationExists_CachesObject()
        {
            await Cache.CacheAsync(User.Test);

            CacheImplementation.Dictionary.ContainsKey(Key).Should().BeTrue();
        }

        [Fact]
        public async Task RetrieveAsync_ConfigurationDoesNotExist_ThrowsException()
        {
            await Cache.Invoking(c => c.CacheAsync(Order.Test))
                .Should().ThrowAsync<ConfigurationNotFoundException>();
        }
    }
}
