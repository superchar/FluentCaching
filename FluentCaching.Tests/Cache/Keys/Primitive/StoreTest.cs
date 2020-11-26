
using System;
using System.Threading.Tasks;
using FluentAssertions;
using FluentCaching.Configuration;
using FluentCaching.Exceptions;
using FluentCaching.Tests.Extensions;
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

            CachingConfiguration.Instance
                .For<User>(u => u.UseAsKey(key).Complete());

            await User.Test.CacheAsync();

            Dictionary.Keys.Should().HaveCount(1).And.Contain(key);

            Dictionary[key].Should().Be(User.Test);
        }

        [Fact]
        public async Task CacheAsync_PropertyKey_KeyIsToProperty()
        {
            CachingConfiguration.Instance
                .For<User>(u => u.UseAsKey(u => u.LastName).Complete());

            await User.Test.CacheAsync();

            var key = User.Test.LastName;

            Dictionary.Keys.Should().HaveCount(1).And.Contain(key);

            Dictionary[key].Should().Be(User.Test);
        }

        [Fact]
        public async Task CacheAsync_SelfKey_KeyIsTakenFromToString()
        {
            CachingConfiguration.Instance
                .For<User>(u => u.UseSelfAsKey().Complete());

            await User.Test.CacheAsync();

            var key = User.Test.ToString();

            Dictionary.Keys.Should().HaveCount(1).And.Contain(key);

            Dictionary[key].Should().Be(User.Test);
        }

        [Fact]
        public async Task CacheAsync_AllPossibleKeyTypes_GeneratesValidKey()
        {
            const string staticKeyPart = "user";

            CachingConfiguration.Instance
                .For<User>(u => u.UseAsKey(staticKeyPart).CombinedWithSelf().CombinedWith(u => u.Id)
                    .Complete());

            var key = $"{staticKeyPart}{User.Test}{User.Test.Id}";

            await User.Test.CacheAsync();

            Dictionary.Keys.Should().HaveCount(1).And.Contain(key);

            Dictionary[key].Should().Be(User.Test);
        }

        [Fact]
        public void CacheAsync_MissingConfiguration_ThrowsException()
        {
            Func<Task> cacheAsync = async () => await User.Test.CacheAsync();

            cacheAsync.Should().Throw<ConfigurationNotFoundException>();

            Dictionary.Keys.Should().BeEmpty();
        }

        [Fact]
        public void CacheAsync_NullKeyPart_ThrowsException()
        {
            var user = User.Test.Clone();

            user.FirstName = null;

            CachingConfiguration.Instance
                .For<User>(u => u.UseAsKey(u => u.FirstName).Complete());

            Func<Task> cacheAsync = async () => await user.CacheAsync();

            cacheAsync.Should().Throw<KeyPartNullException>();

            Dictionary.Keys.Should().BeEmpty();
        }
    }
}
