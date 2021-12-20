using System.Threading.Tasks;
using FluentAssertions;
using Xunit;
using FluentCaching.Tests.Integration.Models;
using FluentCaching.Tests.Integration.Extensions;
using FluentCaching.Tests.Integration.Fakes;

namespace FluentCaching.Tests.Integration.Cache
{
    public class StoreMultipleImplementationsTests : BaseTest
    {
        private readonly DictionaryCacheImplementation _ordersCacheImplementation = new DictionaryCacheImplementation();

        [Fact]
        public async Task CacheAsync_SpecificImplementation_UseSpecificCache()
        {
            const string key = "order";
            var cache = CacheBuilder
                .For<Order>(u => u.UseAsKey(key).Complete(_ordersCacheImplementation))
                .Build();

            await cache.CacheAsync(Order.Test);

            Dictionary.Keys.Should().BeEmpty();
            _ordersCacheImplementation.Dictionary.Keys.Should().HaveCount(1).And.Contain(key);
            _ordersCacheImplementation.Dictionary[key].Should().Be(Order.Test);

        }

        [Fact]
        public async Task CacheAsync_GenericImplementation_UseGenericCache()
        {
            const string orderKey = "order";
            const string userKey = "user";
            var cache = CacheBuilder
                .For<Order>(o => o.UseAsKey(orderKey).Complete(_ordersCacheImplementation))
                .For<User>(u => u.UseAsKey(userKey).Complete())
                .Build();


            await cache.CacheAsync(User.Test);

            _ordersCacheImplementation.Dictionary.Keys.Should().BeEmpty();
            Dictionary.Keys.Should().HaveCount(1).And.Contain(userKey);
            Dictionary[userKey].Should().Be(User.Test);

        }
    }
}
