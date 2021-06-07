using System;
using System.Threading.Tasks;
using FluentAssertions;
using FluentCaching.Exceptions;
using FluentCaching.Tests.Extensions;
using FluentCaching.Tests.Models;
using Xunit;

namespace FluentCaching.Tests.Cache
{
    public class StoreTests : BaseTest
    {
        [Fact]
        public async Task CacheAsync_StaticKey_CachesValue()
        {
            const string key = "user";

            Configuration
                .For<User>(u => u.UseAsKey(key).Complete());

            await User.Test.CacheAsync(Configuration);

            Dictionary.Keys.Should().HaveCount(1).And.Contain(key);

            Dictionary[key].Should().Be(User.Test);
        }

        [Fact]
        public async Task CacheAsync_PropertyKey_CachesValue()
        {
            Configuration
                .For<User>(u => u.UseAsKey(u => u.LastName).Complete());

            await User.Test.CacheAsync(Configuration);

            var key = User.Test.LastName;

            Dictionary.Keys.Should().HaveCount(1).And.Contain(key);

            Dictionary[key].Should().Be(User.Test);
        }

        [Fact]
        public async Task CacheAsync_SelfKey_CachesValue()
        {
            Configuration
                .For<User>(u => u.UseSelfAsKey().Complete());

            await User.Test.CacheAsync(Configuration);

            var key = User.Test.ToString();

            Dictionary.Keys.Should().HaveCount(1).And.Contain(key);

            Dictionary[key].Should().Be(User.Test);
        }

        [Fact]
        public async Task CacheAsync_AllPossibleKeyTypes_CachesValue()
        {
            const string staticKeyPart = "user";

            Configuration
                .For<User>(u => u.UseAsKey(staticKeyPart).CombinedWithSelf().CombinedWith(u => u.Id)
                    .Complete());

            var key = $"{staticKeyPart}{User.Test}{User.Test.Id}";

            await User.Test.CacheAsync(Configuration);

            Dictionary.Keys.Should().HaveCount(1).And.Contain(key);

            Dictionary[key].Should().Be(User.Test);
        }

        [Fact]
        public void CacheAsync_MissingConfiguration_ThrowsException()
        {
            Configuration
                .For<User>(u => u.UseSelfAsKey().CombinedWith(u => u.Id)
                    .Complete());

            Func<Task> cacheAsync = async () => await Order.Test.CacheAsync(Configuration);

            cacheAsync.Should().Throw<ConfigurationNotFoundException>();

            Dictionary.Keys.Should().BeEmpty();
        }

        [Fact]
        public void CacheAsync_NullKeyPart_ThrowsException()
        {
            var user = User.Test.Clone();

            user.FirstName = null;

            Configuration
                .For<User>(u => u.UseAsKey(u => u.FirstName).Complete());

            Func<Task> cacheAsync = async () => await user.CacheAsync(Configuration);

            cacheAsync.Should().Throw<KeyPartNullException>();

            Dictionary.Keys.Should().BeEmpty();
        }
    }
}
