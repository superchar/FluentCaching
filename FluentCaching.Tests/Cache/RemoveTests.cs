using System;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;
using FluentCaching.Exceptions;
using FluentCaching.Tests.Integration.Extensions;
using FluentCaching.Tests.Integration.Models;

namespace FluentCaching.Tests.Integration.Cache
{
    public class RemoveTests : BaseTest
    {
        [Fact]
        public async Task RemoveAsync_StaticKey_RemovesValue()
        {
            const string key = "user";
            var cache = CacheBuilder
                .For<User>(u => u.UseAsKey(key).Complete())
                .Build();

            await cache.CacheAsync(User.Test);
            await cache.RemoveAsync<User>(key);
            var result = await cache.RetrieveAsync<User>(key);

            result.Should().BeNull();
        }

        [Fact]
        public async Task RemoveAsync_SinglePropertyKey_RemovesValue()
        {
            var cache = CacheBuilder
                .For<User>(u => u.UseAsKey(u => u.Id).Complete())
                .Build();

            await cache.CacheAsync(User.Test);
            await cache.RemoveAsync<User>(User.Test.Id);
            var result = await cache.RetrieveAsync<User>(User.Test.Id);

            result.Should().BeNull();
        }

        [Fact]
        public async Task RemoveAsync_MultiPropertyKey_RemovesValue()
        {
            var cache = CacheBuilder
                .For<User>(u => u.UseAsKey(u => u.Id).CombinedWith(u => u.LastName).Complete())
                .Build();

            await cache.CacheAsync(User.Test);
            var key = new { User.Test.Id, User.Test.LastName };
            await cache.RemoveAsync<User>(key);
            var result = await cache.RetrieveAsync<User>(key);

            result.Should().BeNull();
        }

        [Fact]
        public void RemoveAsync_MissingConfiguration_ThrowsException()
        {
            var cache = CacheBuilder
                .For<User>(u => u.UseAsKey("test")
                    .CombinedWith(_ => _.LastName)
                    .CombinedWith(_ => _.Id).Complete())
                .Build();

            Func<Task<Order>> retrieveAsync = async () => await cache.RetrieveAsync<Order>(new { Id = 1, LastName = "Test" });

            retrieveAsync.Should().Throw<ConfigurationNotFoundException>();
        }
    }
}
