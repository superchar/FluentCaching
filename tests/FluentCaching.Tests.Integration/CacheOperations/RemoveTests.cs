using System;
using System.Threading.Tasks;
using FluentAssertions;
using FluentCaching.Configuration.Exceptions;
using FluentCaching.Tests.Integration.Models;
using Moq;
using Xunit;

namespace FluentCaching.Tests.Integration.CacheOperations
{
    public class RemoveTests : CacheOperationBaseTest
    {
        [Fact]
        public async Task RemoveAsync_ConfigurationExists_RemovesObjectFromCache()
        {
            CacheImplementation.Dictionary[Key] = new User();
            
            await Cache.RemoveAsync<User>(Key);

            CacheImplementation.Dictionary.ContainsKey(Key).Should().BeFalse();
        }
        
        [Fact]
        public async Task RemoveAsync_ConfigurationDoesNotExist_ThrowsException()
        {
            await Cache.Invoking(c => c.RetrieveAsync<Order>(new { Id = 1, LastName = "Test" }))
                .Should().ThrowAsync<ConfigurationNotFoundException>();
        }
    }
}
