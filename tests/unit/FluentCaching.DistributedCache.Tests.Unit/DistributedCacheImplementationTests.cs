using System.Text.Json;
using FluentAssertions;
using FluentCaching.Cache.Models;
using Microsoft.Extensions.Caching.Distributed;
using Moq;
using Xunit;

namespace FluentCaching.DistributedCache.Tests.Unit
{
    public class DistributedCacheImplementationTests
    {
        private static readonly User User = new User("Some name");
        
        private readonly Mock<IDistributedCache> _distributedCacheMock;

        private readonly DistributedCacheImplementation _cacheImplementation;

        public DistributedCacheImplementationTests()
        {
            _distributedCacheMock = new Mock<IDistributedCache>();

            _cacheImplementation = new DistributedCacheImplementation(_distributedCacheMock.Object);
        }

        public static IEnumerable<object[]> NullOrEmptyBytes
        {
            get
            {
                yield return new[] { (byte[])null };
                yield return new[] { Array.Empty<byte>() };
            }
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

            var result = await _cacheImplementation.RetrieveAsync<User>("Some key");

            result.Should().BeEquivalentTo(User);
        }

        [Fact]
        public async Task CacheAsync_WhenCalled_IvokesDistributedCache()
        {
            var resultBytes = JsonSerializer.SerializeToUtf8Bytes(User);
            var options = CreateCacheOptions(TimeSpan.FromSeconds(1));

            await _cacheImplementation.CacheAsync("Some key", User, options);

            _distributedCacheMock.Verify(
                c => c.SetAsync("Some key", It.Is<byte[]>(bytes => bytes.SequenceEqual(resultBytes)),
                    It.IsAny<DistributedCacheEntryOptions>(), CancellationToken.None), Times.Once);
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
            => new CacheOptions
            {
                Ttl = ttl,
                ExpirationType = expirationType
            };
    }
}