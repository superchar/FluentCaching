using FluentAssertions;
using FluentCaching.Cache.Models;
using Xunit;

namespace FluentCaching.Memory.Tests.Unit
{
    public class MemoryCacheImplementationTests
    {
        private readonly MemoryCacheImplementation _sut = new();

        [Fact]
        public async Task RetrieveAsync_KeyIsNotInCache_ReturnsNull()
        {
            var result = await _sut.RetrieveAsync<User>("Some key");

            result.Should().BeNull();
        }
        
        [Fact]
        public async Task RetrieveAsync_KeyIsInCache_ReturnsValue()
        {
            var user = new User();
            await _sut.CacheAsync("Some key", user, new CacheOptions());
            
            var result = await _sut.RetrieveAsync<User>("Some key");

            result.Should().BeNull();
        }
        
        [Fact]
        public async Task RetrieveAsync_AbsoluteExpirationWithTtl_RemovesItemAfterTtl()
        {
            var user = new User();
            const int ttlMilliseconds = 100;
            await _sut.CacheAsync("Some key", user, new CacheOptions
            {
                Ttl = TimeSpan.FromMilliseconds(ttlMilliseconds)
            });
            await Task.Delay(ttlMilliseconds * 2);
                
            var result = await _sut.RetrieveAsync<User>("Some key");

            result.Should().BeNull();
        }
        
        [Fact]
        public async Task RetrieveAsync_SlidingExpirationWithTtl_RemovesItemAfterTtl()
        {
            var user = new User();
            const int ttlMilliseconds = 100;
            await _sut.CacheAsync("Some key", user, new CacheOptions
            {
                Ttl = TimeSpan.FromMilliseconds(ttlMilliseconds)
            });
            await Task.Delay(ttlMilliseconds * 2);
                
            var result = await _sut.RetrieveAsync<User>("Some key");

            result.Should().BeNull();
        }
    }
}