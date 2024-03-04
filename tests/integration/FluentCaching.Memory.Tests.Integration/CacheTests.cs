using System.Runtime.Caching;
using FluentAssertions;
using FluentCaching.Cache;
using FluentCaching.Cache.Builders;
using Xunit;

namespace FluentCaching.Memory.Tests.Integration;

public class CacheTests
{
    private const string UserLastName = "User last name";

    private static readonly User User = new()
    {
        FirstName = "User first name",
        LastName = UserLastName
    };

    private ICache _cache;

    public CacheTests()
    {
        _cache = BuildWithScalarCacheConfiguration();
    }

    private static string ComplexUserKey => $"{User.FirstName}:{User.LastName}";
    
    [Fact]
    public async Task CacheObject_CachesObject()
    {
        await _cache.CacheAsync(User);

        var userFromCache = GetUserFromCache();
        userFromCache.Should().NotBeNull();
    }
    
    [Fact]
    public async Task CacheWithDefaultCache_CachesObject()
    {
        _cache = new CacheBuilder()
            .SetInMemoryAsDefaultCache()
            .For<User>(c => c.UseAsKey(u => u.LastName).And()
                .SetInfiniteExpirationTimeout())
            .Build();
        
        await _cache.CacheAsync(User);

        var userFromCache = GetUserFromCache();
        userFromCache.Should().NotBeNull();
    }
    
    [Fact]
    public async Task CacheWithAbsoluteExpiration_RemovesItemAfterTtl()
    {
        const int ttlMilliseconds = 1000;
        _cache = new CacheBuilder()
            .For<User>(_ => _.UseAsKey(u => u.LastName).And()
                .SetExpirationTimeoutTo(ttlMilliseconds / 1000).Seconds.With()
                .AbsoluteExpiration().And()
                .StoreInMemory())
            .Build();
        await _cache.CacheAsync(User);

        var userFromCache = GetUserFromCache();
        userFromCache.Should().NotBeNull();
            
        await Task.Delay(ttlMilliseconds * 2);
        userFromCache = GetUserFromCache();

        userFromCache.Should().BeNull();
    }
    
    [Fact]
    public async Task CacheWithSlidingExpiration_KeepsItemUntilTimeoutExpired()
    {
        const int ttlMilliseconds = 2000;
        _cache = new CacheBuilder()
            .For<User>(_ => _.UseAsKey(u => u.LastName).And()
                .SetExpirationTimeoutTo(ttlMilliseconds / 1000).Seconds.With()
                .SlidingExpiration().And()
                .StoreInMemory())
            .Build();

        await _cache.CacheAsync(User);
        
        var result = await WaitAndRetrieve(ttlMilliseconds / 4);
        result.Should().NotBeNull();
            
        result = await WaitAndRetrieve(ttlMilliseconds / 4);
        result.Should().NotBeNull();
            
        result = await WaitAndRetrieve(ttlMilliseconds / 4);
        result.Should().NotBeNull();
            
        result = await WaitAndRetrieve(ttlMilliseconds / 4);
        result.Should().NotBeNull();
            
        result = await WaitAndRetrieve(ttlMilliseconds / 4);
        result.Should().NotBeNull();

        result = await WaitAndRetrieve(ttlMilliseconds * 2);
        result.Should().BeNull();
    }
    
    [Fact]
    public async Task RetrieveNonExistingKey_ReturnsNull()
    {
        var result = await _cache.RetrieveAsync<User>("Some key");

        result.Should().BeNull();
    }
        
    [Fact]
    public async Task RetrieveScalarKey_RetrievesObject()
    {
        SetUserToCache();
            
        var result = await _cache.RetrieveAsync<User>(UserLastName);

        result.Should().NotBeNull();
    }
    
    [Fact]
    public async Task RetrieveStaticKey_RetrievesObject()
    {
        SetUserToCache();
        _cache = BuildWithStaticCacheConfiguration();
            
        var result = await _cache.RetrieveAsync<User>();

        result.Should().NotBeNull();
    }
    
    [Fact]
    public async Task RetrieveComplexKey_RetrievesObject()
    {
        SetUserToCache(ComplexUserKey);
        _cache = BuildWithComplexCacheConfiguration();
            
        var result = await _cache.RetrieveAsync<User>(new { User.FirstName, User.LastName });

        result.Should().NotBeNull();
    }

    [Fact]
    public async Task RemoveScalarKey_RemovesKey()
    {
        SetUserToCache();

        await _cache.RemoveAsync<User>(UserLastName);

        var userFromCache = GetUserFromCache();
        userFromCache.Should().BeNull();
    }
    
    [Fact]
    public async Task RemoveStaticKey_RemovesKey()
    {
        _cache = BuildWithStaticCacheConfiguration();
        SetUserToCache();

        await _cache.RemoveAsync<User>();

        var userFromCache = GetUserFromCache();
        userFromCache.Should().BeNull();
    }
    
    [Fact]
    public async Task RemoveComplexKey_RemovesKey()
    {
        _cache = BuildWithComplexCacheConfiguration();
        SetUserToCache(ComplexUserKey);

        await _cache.RemoveAsync<User>(new { User.FirstName, User.LastName });

        var userFromCache = GetUserFromCache(ComplexUserKey);
        userFromCache.Should().BeNull();
    }

    private async Task<User?> WaitAndRetrieve(int timeout)
    {
        await Task.Delay(timeout);
        return await _cache.RetrieveAsync<User>(UserLastName);   
    }

    private static ICache BuildWithScalarCacheConfiguration()
        => new CacheBuilder()
            .For<User>(_ => _.UseAsKey(u => u.LastName).And()
                .SetInfiniteExpirationTimeout().And()
                .StoreInMemory())
            .Build();

    private static ICache BuildWithStaticCacheConfiguration()
        => new CacheBuilder()
            .For<User>(_ => _.UseAsKey(UserLastName).And()
                .SetInfiniteExpirationTimeout().And()
                .StoreInMemory())
            .Build();

    private static ICache BuildWithComplexCacheConfiguration()
        => new CacheBuilder()
            .For<User>(_ => _.UseAsKey(u => u.FirstName).CombinedWith(u => u.LastName).And()
                .SetInfiniteExpirationTimeout().And()
                .StoreInMemory())
            .Build();

    private static void SetUserToCache(string? key = null)
        => MemoryCache.Default.Set(key ?? UserLastName, User, DateTimeOffset.MaxValue);

    private static User? GetUserFromCache(string? key = null)
        => MemoryCache.Default.Get(key ?? UserLastName) as User;
}
