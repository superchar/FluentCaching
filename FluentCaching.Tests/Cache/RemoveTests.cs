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

            Configuration
                .For<User>(u => u.UseAsKey(key).Complete());

            await User.Test.CacheAsync(Configuration);

            await key.RemoveAsync<User>(Configuration);

            var result = await key.RetrieveAsync<User>(Configuration);

            result.Should().BeNull();
        }

        [Fact]
        public async Task RemoveAsync_SelfKey_RemovesValue()
        {
            Configuration
                .For<User>(u => u.UseSelfAsKey().Complete());

            await User.Test.CacheAsync(Configuration);

            var selfKey = User.Test.ToString();

            await selfKey.RemoveAsync<User>(Configuration);

            var result = await selfKey.RetrieveAsync<User>(Configuration);

            result.Should().BeNull();
        }

        [Fact]
        public async Task RemoveAsync_SinglePropertyKey_RemovesValue()
        {
            Configuration
                .For<User>(u => u.UseAsKey(u => u.Id).Complete());

            await User.Test.CacheAsync(Configuration);

            await User.Test.Id.RemoveAsync<User>(Configuration);

            var result = await User.Test.Id.RetrieveAsync<User>(Configuration);

            result.Should().BeNull();
        }

        [Fact]
        public async Task RemoveAsync_MultiPropertyKey_RemovesValue()
        {
            Configuration
                .For<User>(u => u.UseAsKey(u => u.Id).CombinedWith(u => u.LastName).Complete());

            await User.Test.CacheAsync(Configuration);

            var key = new { User.Test.Id, User.Test.LastName };

            await key.RemoveAsync<User>(Configuration);

            var result = await key.RetrieveAsync<User>(Configuration);

            result.Should().BeNull();
        }

        [Fact]
        public void RemoveAsync_MissingConfiguration_ThrowsException()
        {
            Configuration
                .For<User>(u => u.UseSelfAsKey()
                    .CombinedWith(_ => _.LastName)
                    .CombinedWith(_ => _.Id).Complete());

            Func<Task<Order>> retrieveAsync = async () => await new { Id = 1, LastName = "Test" }.RetrieveAsync<Order>(Configuration);

            retrieveAsync.Should().Throw<ConfigurationNotFoundException>();
        }
    }
}
