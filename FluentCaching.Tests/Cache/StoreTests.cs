using System;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;
using FluentCaching.Exceptions;
using FluentCaching.Tests.Integration.Models;
using FluentCaching.Tests.Integration.Extensions;

namespace FluentCaching.Tests.Integration.Cache
{
    public class StoreTests : BaseTest
    {
        [Fact]
        public async Task CacheAsync_StaticKey_CachesValue()
        {
            const string key = "user";
            var cache = CacheBuilder
                .For<User>(u => u.UseAsKey(key).Complete())
                .Build();

            await cache.CacheAsync(User.Test);

            Dictionary.Keys.Should().HaveCount(1).And.Contain(key);
            Dictionary[key].Should().Be(User.Test);
        }

        [Fact]
        public async Task CacheAsync_PropertyKey_CachesValue()
        {
            var cache = CacheBuilder
                .For<User>(u => u.UseAsKey(u => u.LastName).Complete())
                .Build();

            await cache.CacheAsync(User.Test);

            var key = User.Test.LastName;
            Dictionary.Keys.Should().HaveCount(1).And.Contain(key);
            Dictionary[key].Should().Be(User.Test);
        }

        [Fact]
        public async Task CacheAsync_AllPossibleKeyTypes_CachesValue()
        {
            const string staticKeyPart = "user";
            var cache = CacheBuilder
                .For<User>(u => u.UseAsKey(staticKeyPart).CombinedWith(u => u.Id)
                    .Complete())
                .Build();

            var key = $"{staticKeyPart}{User.Test.Id}";
            await cache.CacheAsync(User.Test);

            Dictionary.Keys.Should().HaveCount(1).And.Contain(key);
            Dictionary[key].Should().Be(User.Test);
        }

        [Fact]
        public void CacheAsync_MissingConfiguration_ThrowsException()
        {
             var cache = CacheBuilder
                .For<User>(u => u.UseAsKey("test").CombinedWith(u => u.Id).Complete())
                .Build();

            Func<Task> cacheAsync = async () => await cache.CacheAsync(Order.Test);

            cacheAsync.Should().Throw<ConfigurationNotFoundException>();
            Dictionary.Keys.Should().BeEmpty();
        }

        [Fact]
        public void CacheAsync_NullKeyPart_ThrowsException()
        {
            var user = User.Test.Clone();
            user.FirstName = null;
            var cache = CacheBuilder
               .For<User>(u => u.UseAsKey(u => u.FirstName).Complete())
               .Build();

            Func<Task> cacheAsync = async () => await cache.CacheAsync(user);

            cacheAsync.Should().Throw<KeyPartNullException>();
            Dictionary.Keys.Should().BeEmpty();
        }
    }
}
