using System.Text.Json;
using FluentAssertions;
using FluentCaching.Cache.Models;
using FluentCaching.Keys.Builders;
using Microsoft.Extensions.Caching.Distributed;
using Moq;
using Xunit;
// ReSharper disable CoVariantArrayConversion

namespace FluentCaching.DistributedCache.Tests.Unit;

public class DistributedCacheImplementationTests
{
    private static readonly User User = new ("User Name");
        
    private readonly Mock<IDistributedCache> _distributedCacheMock;
    private readonly Mock<IDistributedCacheSerializer> _distributedCacheSerializerMock;

    private readonly DistributedCacheImplementation _cacheImplementation;

    public DistributedCacheImplementationTests()
    {
        _distributedCacheMock = new Mock<IDistributedCache>();
        _distributedCacheSerializerMock = new Mock<IDistributedCacheSerializer>();

        _distributedCacheSerializerMock
            .Setup(s => s.CanBeUsedForType(typeof(User)))
            .Returns(true);

        _cacheImplementation = new DistributedCacheImplementation(_distributedCacheMock.Object,
            [_distributedCacheSerializerMock.Object]);
    }

    public static IEnumerable<object[]> NullOrEmptyBytes
    {
        get
        {
            yield return [null];
            yield return [Array.Empty<byte>()];
        }
    }
    
    [Fact]
    public async Task RetrieveAsync_WhenCalled_InvokesDistributedCache()
    {
        var resultBytes = JsonSerializer.SerializeToUtf8Bytes(User);
        _distributedCacheMock
            .Setup(c => c.GetAsync("Some key", CancellationToken.None))
            .ReturnsAsync(resultBytes);

        await _cacheImplementation.RetrieveAsync<User>("Some key");

        _distributedCacheMock.Verify(d => d.GetAsync("Some key", CancellationToken.None), Times.Once);
    }
    
    [Fact]
    public async Task RetrieveAsync_WhenCalled_DistributedCacheSerializer()
    {
        var resultBytes = JsonSerializer.SerializeToUtf8Bytes(User);
        _distributedCacheMock
            .Setup(c => c.GetAsync("Some key", CancellationToken.None))
            .ReturnsAsync(resultBytes);

        await _cacheImplementation.RetrieveAsync<User>("Some key");

        _distributedCacheSerializerMock.Verify(s => s.DeserializeAsync<User>(resultBytes), Times.Once);
    }

    [Theory]
    [MemberData(nameof(NullOrEmptyBytes))]
    public async Task RetrieveAsync_KeyIsNotInCache_ReturnsNull(byte[] resultBytes)
    {
        _distributedCacheMock
            .Setup(c => c.GetAsync("Some key", CancellationToken.None))
            .ReturnsAsync(resultBytes);

        var result = await _cacheImplementation.RetrieveAsync<User>("Some key");

        result.Should().BeNull();
    }

    [Fact]
    public async Task RetrieveAsync_KeyIsInCache_ReturnsValue()
    {
        var resultBytes = JsonSerializer.SerializeToUtf8Bytes(User);
        _distributedCacheMock
            .Setup(c => c.GetAsync("Some key", CancellationToken.None))
            .ReturnsAsync(resultBytes);
        _distributedCacheSerializerMock
            .Setup(s => s.DeserializeAsync<User>(resultBytes))
            .ReturnsAsync(User);

        var result = await _cacheImplementation.RetrieveAsync<User>("Some key");

        result.Should().BeEquivalentTo(User);
    }

    [Fact]
    public async Task CacheAsync_WhenCalled_InvokesDistributedCache()
    {
        var resultBytes = JsonSerializer.SerializeToUtf8Bytes(User);
        var options = CreateCacheOptions(TimeSpan.FromSeconds(1));
        _distributedCacheSerializerMock
            .Setup(s => s.SerializeAsync(User))
            .ReturnsAsync(resultBytes);

        await _cacheImplementation.CacheAsync("Some key", User, options);

        _distributedCacheMock.Verify(
            c => c.SetAsync("Some key", It.Is<byte[]>(bytes => bytes.SequenceEqual(resultBytes)),
                It.IsAny<DistributedCacheEntryOptions>(), CancellationToken.None), Times.Once);
    }
    
    [Fact]
    public async Task CacheAsync_WhenCalled_InvokesDistributedCacheSerializer()
    {
        var options = CreateCacheOptions(TimeSpan.FromSeconds(1));

        await _cacheImplementation.CacheAsync("Some key", User, options);

        _distributedCacheSerializerMock.Verify(s => s.SerializeAsync(User), Times.Once);
    }

    [Fact]
    public async Task CacheAsync_InfiniteExpiration_SetsInfiniteExpirationInCache()
    {
        var options = CreateCacheOptions(TimeSpan.MaxValue);

        await _cacheImplementation.CacheAsync("Some key", User, options);

        _distributedCacheMock.Verify(
            c => c.SetAsync("Some key", It.IsAny<byte[]>(),
                It.Is<DistributedCacheEntryOptions>(
                    o => o.AbsoluteExpirationRelativeToNow == null && o.SlidingExpiration == null), CancellationToken.None),
            Times.Once);
    }
        
    [Fact]
    public async Task CacheAsync_AbsoluteExpiration_SetsInfiniteExpirationInCache()
    {
        var options = CreateCacheOptions(TimeSpan.FromSeconds(5));

        await _cacheImplementation.CacheAsync("Some key", User, options);

        _distributedCacheMock.Verify(
            c => c.SetAsync("Some key", It.IsAny<byte[]>(),
                It.Is<DistributedCacheEntryOptions>(
                    o => o.AbsoluteExpirationRelativeToNow == options.Ttl && o.SlidingExpiration == null), CancellationToken.None),
            Times.Once);
    }
        
    [Fact]
    public async Task CacheAsync_SlidingExpiration_SetsInfiniteExpirationInCache()
    {
        var options = CreateCacheOptions(TimeSpan.FromSeconds(5), ExpirationType.Sliding);

        await _cacheImplementation.CacheAsync("Some key", User, options);

        _distributedCacheMock.Verify(
            c => c.SetAsync("Some key", It.IsAny<byte[]>(),
                It.Is<DistributedCacheEntryOptions>(
                    o => o.SlidingExpiration == options.Ttl && o.AbsoluteExpirationRelativeToNow == null), CancellationToken.None),
            Times.Once);
    }

    [Fact]
    public async Task RemoveAsync_WhenCalled_InvokesDistributedCache()
    {
        await _cacheImplementation.RemoveAsync("Some key");

        _distributedCacheMock.Verify(c => c.RemoveAsync("Some key", CancellationToken.None), Times.Once);
    }

    private static CacheOptions CreateCacheOptions(TimeSpan ttl,
        ExpirationType expirationType = ExpirationType.Absolute)
        => new (new Mock<IKeyBuilder>().Object)
        {
            Ttl = ttl,
            ExpirationType = expirationType
        };
}