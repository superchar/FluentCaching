using FluentAssertions;
using FluentCaching.Cache.Models;
using FluentCaching.Keys.Builders;
using Moq;
using Xunit;

namespace FluentCaching.Memory.Tests.Integration;

public class MemoryCacheImplementationTests
{
    private static readonly User User = new ("Some name");
        
    
    private readonly MemoryCacheImplementation _sut = new();

    [Fact]
    public async Task KeyIsNotInCache_ReturnsNull()
    {
        var result = await _sut.RetrieveAsync<User>("Some key");

        result.Should().BeNull();
    }
        
    [Fact]
    public async Task KeyIsInCache_ReturnsValue()
    {
        await _sut.CacheAsync("Some key", User, CreateCacheOptions(TimeSpan.MaxValue));
            
        var result = await _sut.RetrieveAsync<User>("Some key");

        result.Should().NotBeNull();
    }
        
    [Fact]
    public async Task AbsoluteExpirationWithTtl_RemovesItemAfterTtl()
    {
        const int ttlMilliseconds = 1000;
        await _sut.CacheAsync("Some key", User, CreateCacheOptions(TimeSpan.FromMilliseconds(ttlMilliseconds)));
        var result = await _sut.RetrieveAsync<User>("Some key");
        result.Should().NotBeNull();
            
        await Task.Delay(ttlMilliseconds * 2);
        result = await _sut.RetrieveAsync<User>("Some key");

        result.Should().BeNull();
    }
        
    [Fact]
    public async Task SlidingExpirationWithTtl_KeepItemUntilTimeoutExpired()
    {
        const int ttlMilliseconds = 2000;
        await _sut.CacheAsync("Some key", User,
            CreateCacheOptions(TimeSpan.FromMilliseconds(ttlMilliseconds), ExpirationType.Sliding));

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
    public async Task KeyIsInCache_RemoveKey()
    {
        await _sut.CacheAsync("Some key", User, CreateCacheOptions(TimeSpan.MaxValue));
            
        var result = await _sut.RetrieveAsync<User>("Some key");
        result.Should().NotBeNull();
            
        await _sut.RemoveAsync("Some key");
        result = await _sut.RetrieveAsync<User>("Some key");
        result.Should().BeNull();
    }

    private async Task<User?> WaitAndRetrieve(int timeout)
    {
        await Task.Delay(timeout);
        return await _sut.RetrieveAsync<User>("Some key");   
    }

    private static CacheOptions CreateCacheOptions(TimeSpan ttl, ExpirationType expirationType = default)
        => new (new Mock<IKeyBuilder>().Object)
        {
            Ttl = ttl,
            ExpirationType = expirationType
        };
}