using FluentAssertions;
using FluentCaching.Cache.Models;
using FluentCaching.Configuration.PolicyBuilders;
using Xunit;

namespace FluentCaching.Memory.Tests.Unit
{
    public class CacheImplementationBuilderExtensionsTests 
    {
        [Fact]
        public void StoreInMemory_WhenCalled_ReturnsCacheImplementationPolicyBuilder()
        {
            var builder = new CacheImplementationPolicyBuilder(new CacheOptions());

            var result = builder.StoreInMemory();

            result.Should().NotBeNull();
        }
    }
}