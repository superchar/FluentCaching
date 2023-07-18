using System.Threading.Tasks;
using FluentAssertions;
using FluentCaching.Cache;
using FluentCaching.Cache.Models;
using FluentCaching.Configuration.Exceptions;
using FluentCaching.Tests.Integration.Extensions;
using FluentCaching.Tests.Integration.Models;
using Xunit;

namespace FluentCaching.Tests.Integration.Cache;

public class CacheTests : BaseTest
{
    private const string DefaultPolicyKey = "user";

    private const string CustomPolicyKey = "user_custom_policy";
    private const string CustomPolicyName = "custom_policy_name";

    private readonly ICache _cache;

    public CacheTests()
    {
        _cache = CacheBuilder
            .For<User>(u => u.UseAsKey(DefaultPolicyKey).Complete())
            .For<User>(u => u.UseAsKey(CustomPolicyKey).And(CustomPolicyName).PolicyName().Complete())
            .Build();
    }

    [Fact]
    public async Task CacheConfiguredObject_CachesObject()
    {
        await _cache.CacheAsync(User.Test);

        CacheImplementation.Dictionary.ContainsKey(DefaultPolicyKey).Should().BeTrue();
    }

    [Fact]
    public async Task CacheWithCustomPolicy_CachesObject()
    {
        await _cache.CacheAsync(User.Test, new PolicyName(CustomPolicyName));

        CacheImplementation.Dictionary.ContainsKey(CustomPolicyKey).Should().BeTrue();
    }
    
    [Fact]
    public async Task CacheWithUnknownPolicy_ThrowsException()
    {
        await _cache.Invoking(c => c.CacheAsync(User.Test, new PolicyName("Unknown")).AsTask())
            .Should().ThrowAsync<ConfigurationNotFoundException>();
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
        CacheImplementation.Dictionary[DefaultPolicyKey] = new User();

        await _cache.RemoveAsync<User>(DefaultPolicyKey);

        CacheImplementation.Dictionary.ContainsKey(DefaultPolicyKey).Should().BeFalse();
    }

    [Fact]
    public async Task RemoveWithCustomPolicy_RemovesObjectFromCache()
    {
        CacheImplementation.Dictionary[CustomPolicyKey] = new User();

        await _cache.RemoveAsync<User>(DefaultPolicyKey, new PolicyName(CustomPolicyName));

        CacheImplementation.Dictionary.ContainsKey(DefaultPolicyKey).Should().BeFalse();
    }

    [Fact]
    public async Task RemoveWithUnknownPolicy_ThrowsException()
    {
        CacheImplementation.Dictionary[CustomPolicyKey] = new User();

        await _cache.Invoking(c => c.RemoveAsync<User>(CustomPolicyKey, new PolicyName("Unknown")).AsTask())
            .Should().ThrowAsync<ConfigurationNotFoundException>();
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
        CacheImplementation.Dictionary[DefaultPolicyKey] = user;

        var result = await _cache.RetrieveAsync<User>(DefaultPolicyKey);

        result.Should().Be(user);
    }

    [Fact]
    public async Task RetrieveWithCustomPolicy_RetrievesObjectFromCache()
    {
        var user = new User();
        CacheImplementation.Dictionary[CustomPolicyKey] = user;

        var result = await _cache.RetrieveAsync<User>(CustomPolicyKey, new PolicyName(CustomPolicyName));

        result.Should().Be(user);
    }

    [Fact]
    public async Task RetrieveWithUnknownPolicy_ThrowsException()
    {
        await _cache.Invoking(c => c.RetrieveAsync<User>(CustomPolicyKey, new PolicyName("Unknown")).AsTask())
            .Should().ThrowAsync<ConfigurationNotFoundException>();
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
        var retrievedUser = await cache.RetrieveAsync<User>(DefaultPolicyKey);
        var retrievedOrder = await cache.RetrieveAsync<Order>(Order.Test.OrderId);

        retrievedUser.Should().NotBeNull();
        retrievedOrder.Should().NotBeNull();
    }
}