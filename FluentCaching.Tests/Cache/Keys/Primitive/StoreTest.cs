
using System.Threading.Tasks;
using FluentAssertions;
using FluentCaching.Configuration;
using FluentCaching.Tests.Models;
using Xunit;

namespace FluentCaching.Tests.Cache.Keys.Primitive
{
    public class StoreTest : BaseTest
    {
        [Fact]
        public async Task CacheAsync_StaticKey_KeyIsSetAsStatic()
        {
            const string key = "user";

            CachingConfiguration.Instance.For<User>(userBuilder => 
                userBuilder.UseAsKey(key).And().WithTtlOf(5).Seconds.And().SlidingExpiration());

            var user = User.Test;

            await user.CacheAsync();

            Dictionary.Keys.Should().HaveCount(1).And.Contain(key);

            Dictionary[key].Should().Be(User.Test);
        }
    }
}
