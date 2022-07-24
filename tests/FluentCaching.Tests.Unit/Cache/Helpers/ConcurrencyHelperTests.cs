using FluentAssertions;
using FluentCaching.Cache.Helpers;
using System;
using Xunit;

namespace FluentCaching.Tests.Unit.Cache.Helpers
{
    public class ConcurrencyHelperTests
    {
        [Theory]
        [InlineData(1, 42, 0)]
        [InlineData(1, int.MaxValue, 0)]
        [InlineData(1, 0, 0)]
        [InlineData(5, 42, 2)]
        [InlineData(6, int.MaxValue, 1)]
        [InlineData(10, 578, 8)]
        public void TakeKeyLock_HappyPath_ReturnsCorrectBucket(int locksCount, int key, uint expectedBucket)
        {
            var sut = new ConcurrencyHelper(locksCount);

            var bucket = sut.TakeKeyLock(key);

            bucket.Should().Be(expectedBucket);
        }

        [Theory]
        [InlineData(5, 4)]
        [InlineData(10, 9)]
        [InlineData(15, 0)]
        [InlineData(20, 5)]
        public void ReleaseKeyLock_HappyPath_CompletesSuccessfully(int locksCount, uint keyBucket)
        {
            var sut = new ConcurrencyHelper(locksCount);

            sut
                .Invoking(s => s.ReleaseKeyLock(keyBucket))
                .Should().NotThrow();
        }

        [Theory]
        [InlineData(10, int.MaxValue)]
        [InlineData(15, 16)]
        [InlineData(20, 20)]
        public void ReleaseKeyLock_BucketIsOutOfLocksRange_ThrowsException(int locksCount, uint keyBucket)
        {
            var sut = new ConcurrencyHelper(locksCount);

            sut
                .Invoking(s => s.ReleaseKeyLock(keyBucket))
                .Should()
                .Throw<ArgumentException>().WithMessage("CachedObject bucket is out of locks range (Parameter 'keyBucket')");
        }
    }
}
