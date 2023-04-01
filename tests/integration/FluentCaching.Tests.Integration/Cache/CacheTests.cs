using System.Threading.Tasks;
using FluentAssertions;
using FluentCaching.Cache;
using FluentCaching.Configuration.Exceptions;
using FluentCaching.Tests.Integration.Extensions;
using FluentCaching.Tests.Integration.Models;
using Xunit;

namespace FluentCaching.Tests.Integration.Cache;

public class CacheTests : BaseTest
{
    private const string Key = "user";

    private readonly ICache _cache;

    public CacheTests()
    {
        _cache = CacheBuilder
            .For<User>(u => u.UseAsKey(Key).Complete())
            .Build();
    }

    [Fact]
    public async Task CacheConfiguredObject_CachesObject()
    {
        await _cache.CacheAsync(User.Test);

        CacheImplementation.Dictionary.ContainsKey(Key).Should().BeTrue();
    }

    [Fact]
    public async Task CacheNonConfiguredObject_ThrowsException()
    {
        await _cache.Invoking(c => c.CacheAsync(Order.Test).AsTask())
            .Should().ThrowAsync<ConfigurationNotFoundException>();
    }

    [Fact]
    public async Task RemoveConfiguredObject_RemovesObjectFromCache()
    {
        CacheImplementation.Dictionary[Key] = new User();

        await _cache.RemoveAsync<User>(Key);

        CacheImplementation.Dictionary.ContainsKey(Key).Should().BeFalse();
    }

    [Fact]
    public async Task RemoveNotConfiguredObject_ThrowsException()
    {
        await _cache.Invoking(c => c.RemoveAsync<Order>(new { Id = 1, LastName = "Test" }).AsTask())
            .Should().ThrowAsync<ConfigurationNotFoundException>();
    }

    [Fact]
    public async Task RetrieveConfiguredObject_RetrievesObjectFromCache()
    {
        var user = new User();
        CacheImplementation.Dictionary[Key] = user;

        var result = await _cache.RetrieveAsync<User>(Key);

        result.Should().Be(user);
    }

    [Fact]
    public async Task RetrieveNotConfiguredObject_ThrowsException()
    {
        await _cache.Invoking(c => c.RetrieveAsync<Order>(new { Id = 1, LastName = "Test" }).AsTask())
            .Should().ThrowAsync<ConfigurationNotFoundException>();
    }

    [Fact]
    public async Task CacheWithMultipleConfigurations_SupportsMultipleConfigurations()
    {
        var cache = CacheBuilder
            .For<Order>(_ => _.UseAsKey(o => o.OrderId).Complete())
            .Build();

        await cache.CacheAsync(new User());
        await cache.CacheAsync(Order.Test);
        var retrievedUser = await cache.RetrieveAsync<User>(Key);
        var retrievedOrder = await cache.RetrieveAsync<Order>(Order.Test.OrderId);

        retrievedUser.Should().NotBeNull();
        retrievedOrder.Should().NotBeNull();
    }
}