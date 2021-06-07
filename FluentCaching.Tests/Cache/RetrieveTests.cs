using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using FluentCaching.Exceptions;
using FluentCaching.Tests.Extensions;
using FluentCaching.Tests.Models;
using Xunit;

namespace FluentCaching.Tests.Cache
{
    public class RetrieveTests : BaseTest
    {
        [Fact]
        public async Task RetrieveAsync_StaticKey_RetrievesValue()
        {
            const string key = "user";

            Configuration
                .For<User>(u => u.UseAsKey(key).Complete());

            await User.Test.CacheAsync(Configuration);

            var result = await key.RetrieveAsync<User>(Configuration);

            result.Should().Be(User.Test);
        }

        [Fact]
        public async Task RetrieveAsync_SelfKey_RetrievesValue()
        {
            Configuration
                .For<User>(u => u.UseSelfAsKey().Complete());

            await User.Test.CacheAsync(Configuration);

            var result = await User.Test.ToString().RetrieveAsync<User>(Configuration);

            result.Should().Be(User.Test);
        }

        [Fact]
        public async Task RetrieveAsync_SinglePropertyKey_RetrievesValue()
        {
            Configuration
                .For<User>(u => u.UseAsKey(u => u.Id).Complete());

            await User.Test.CacheAsync(Configuration);

            var result = await User.Test.Id.RetrieveAsync<User>(Configuration);

            result.Should().Be(User.Test);
        }

        [Fact]
        public async Task RetrieveAsync_MultiPropertyAnonymousKey_RetrievesValue()
        {
            Configuration
                .For<User>(u => u.UseAsKey(u => u.Id).CombinedWith(u => u.LastName).Complete());

            await User.Test.CacheAsync(Configuration);

            var key = new {User.Test.Id, User.Test.LastName};

            var result = await key.RetrieveAsync<User>(Configuration);

            result.Should().Be(User.Test);
        }

        [Fact]
        public async Task RetrieveAsync_MultiPropertyKeyWithStaticPart_GeneratesCombinedKeyAndRetrievesValue()
        {
            const string keyPrefix = "userprefix";

            Configuration
                .For<User>(u => u.UseAsKey(keyPrefix)
                    .CombinedWith(u => u.LastName)
                    .CombinedWith(u => u.Id).Complete());

            await User.Test.CacheAsync(Configuration);

            var key = new {User.Test.Id, User.Test.LastName};

            var result = await key.RetrieveAsync<User>(Configuration);

            result.Should().Be(User.Test);

            var expectedStringKey = $"{keyPrefix}{User.Test.LastName}{User.Test.Id}";

            Dictionary.Keys.Single().Should().Be(expectedStringKey);
        }

        [Fact]
        public async Task RetrieveAsync_MultiPropertyKeyWithSelf_GeneratesCombinedKeyAndRetrievesValue()
        {
            Configuration
                .For<User>(u => u.UseSelfAsKey()
                    .CombinedWith(u => u.LastName)
                    .CombinedWith(u => u.Id).Complete());

            await User.Test.CacheAsync(Configuration);

            var key = new {Self = User.Test.ToString(), User.Test.Id, User.Test.LastName};

            var result = await key.RetrieveAsync<User>(Configuration);

            result.Should().Be(User.Test);

            var expectedStringKey = $"{User.Test.ToString()}{User.Test.LastName}{User.Test.Id}";

            Dictionary.Keys.Single().Should().Be(expectedStringKey);
        }

        [Fact]
        public void RetrieveAsync_MissingConfiguration_ThrowsException()
        {
            Configuration
                .For<User>(u => u.UseSelfAsKey()
                    .CombinedWith(_ => _.LastName)
                    .CombinedWith(_ => _.Id).Complete());

            Func<Task<Order>> retrieveAsync = async () => await new {Id = 1, LastName = "Test"}.RetrieveAsync<Order>(Configuration);

            retrieveAsync.Should().Throw<ConfigurationNotFoundException>();

            Dictionary.Keys.Should().BeEmpty();
        }

        [Fact]
        public void RetrieveAsync_WrongKeySchema_ThrowsException()
        {
            Configuration
                .For<User>(u => u.UseAsKey(_ => _.FirstName).CombinedWith(_ => _.LastName).Complete());

            Func<Task<User>> retrieveAsync = async () => await Order.Test.RetrieveAsync<User>(Configuration);

            retrieveAsync.Should().Throw<KeyNotFoundException>()
                .WithMessage("Key schema is not correct");

            Dictionary.Keys.Should().BeEmpty();
        }
    }
}
