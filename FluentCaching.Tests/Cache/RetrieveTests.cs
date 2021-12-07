using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;
using FluentCaching.Exceptions;
using FluentCaching.Tests.Integration.Models;
using FluentCaching.Tests.Integration.Extensions;

namespace FluentCaching.Tests.Integration.Cache
{
    public class RetrieveTests : BaseTest
    {
        [Fact]
        public async Task RetrieveAsync_StaticKey_RetrievesValue()
        {
            const string key = "user";
            var cache = CacheBuilder
                .For<User>(u => u.UseAsKey(key).Complete())
                .Build();

            await cache.CacheAsync(User.Test);
            var result = await cache.RetrieveAsync<User>(key);

            result.Should().Be(User.Test);
        }

        [Fact]
        public async Task RetrieveAsync_SinglePropertyKey_RetrievesValue()
        {
            var cache = CacheBuilder
                .For<User>(u => u.UseAsKey(u => u.Id).Complete()).Build();

            await cache.CacheAsync(User.Test);
            var result = await cache.RetrieveAsync<User>(User.Test.Id);

            result.Should().Be(User.Test);
        }

        [Fact]
        public async Task RetrieveAsync_MultiPropertyAnonymousKey_RetrievesValue()
        {
            var cache = CacheBuilder
                .For<User>(u => u.UseAsKey(u => u.Id).CombinedWith(u => u.LastName).Complete())
                .Build();

            await cache.CacheAsync(User.Test);
            var key = new {User.Test.Id, User.Test.LastName};
            var result = await cache.RetrieveAsync<User>(key);

            result.Should().Be(User.Test);
        }

        [Fact]
        public async Task RetrieveAsync_MultiPropertyKeyWithStaticPart_GeneratesCombinedKeyAndRetrievesValue()
        {
            const string keyPrefix = "userprefix";
            var cache = CacheBuilder
                .For<User>(u => u.UseAsKey(keyPrefix)
                    .CombinedWith(u => u.LastName)
                    .CombinedWith(u => u.Id).Complete())
                .Build();


            await cache.CacheAsync(User.Test);
            var key = new {User.Test.Id, User.Test.LastName};
            var result = await cache.RetrieveAsync<User>(key);

            result.Should().Be(User.Test);
            var expectedStringKey = $"{keyPrefix}{User.Test.LastName}{User.Test.Id}";
            Dictionary.Keys.Single().Should().Be(expectedStringKey);
        }

        [Fact]
        public void RetrieveAsync_MissingConfiguration_ThrowsException()
        {
            var cache = CacheBuilder
                .For<User>(u => u.UseAsKey("test")
                    .CombinedWith(_ => _.LastName)
                    .CombinedWith(_ => _.Id).Complete())
                .Build();

            Func<Task<Order>> retrieveAsync = async () => await cache.RetrieveAsync<Order>(new { Id = 1, LastName = "Test" });
            
            retrieveAsync.Should().Throw<ConfigurationNotFoundException>();
            Dictionary.Keys.Should().BeEmpty();
        }

        [Fact]
        public void RetrieveAsync_WrongKeySchema_ThrowsException()
        {
            var cache = CacheBuilder
                .For<User>(u => u.UseAsKey(_ => _.FirstName).CombinedWith(_ => _.LastName).Complete())
                .Build();

            Func<Task<User>> retrieveAsync = async () => await cache.RetrieveAsync<User>(Order.Test);

            retrieveAsync.Should().Throw<KeyNotFoundException>()
                .WithMessage("Key schema is not correct");
            Dictionary.Keys.Should().BeEmpty();
        }
    }
}
