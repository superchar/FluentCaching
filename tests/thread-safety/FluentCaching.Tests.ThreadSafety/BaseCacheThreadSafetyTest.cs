using FluentAssertions;
using FluentCaching.Cache;
using FluentCaching.Cache.Builders;
using Xunit;

namespace FluentCaching.Tests.ThreadSafety
{
    public abstract class BaseCacheThreadSafetyTest
    {
        protected abstract ICacheImplementation CacheImplementation { get; }

        [Fact]
        public async Task SimpleyKeyTypeWithTheSameKey()
        {
            var cache = new CacheBuilder()
                .For<User>(_ => _.UseAsKey(u => u.Id).And()
                    .SetInfiniteExpirationTimeout().And()
                    .StoreIn(CacheImplementation))
                .Build();

            var users = GenerateUsers(_ => (42, "Some Name"));
            var autoResetEvent = new AutoResetEvent(false);

            var backgroundThread = RunBackgroundСacheOperations(autoResetEvent, cache, users);

            await RunCacheOperations(users, cache, async operationType =>
                {
                    switch (operationType)
                    {
                        case CacheOperationType.Cache:
                            autoResetEvent.Set();
                            break;
                        case CacheOperationType.Retrieve:
                        {
                            var user = await cache.RetrieveAsync<User>(42);
                            user.Should().NotBeNull();
                            autoResetEvent.Set();
                            break;
                        }
                        case CacheOperationType.Remove:
                            autoResetEvent.Set();
                            backgroundThread.Join();
                            var removedUser = await cache.RetrieveAsync<User>(42);
                            removedUser.Should().BeNull();
                            break;
                    }
                },
                (users.Count / 2, users.Count));
        }

        private static Thread RunBackgroundСacheOperations(AutoResetEvent autoResetEvent, ICache cache,
            List<User> users)
        {
            var thread = new Thread(async () =>
            {
                await RunCacheOperations(users, cache, _ => Task.FromResult(autoResetEvent.WaitOne()),
                    (0, users.Count / 2));
            });
            thread.Start();

            return thread;
        }

        private static async Task RunCacheOperations(List<User> users,
            ICache cache,
            Func<CacheOperationType, Task> afterOperationCallback,
            (int Start, int End ) processingRange)
        {
            for (var i = processingRange.Start; i < processingRange.End; i++)
            {
                await cache.CacheAsync(users[i]);
            }

            await afterOperationCallback(CacheOperationType.Cache);

            for (var i = processingRange.Start; i < processingRange.End; i++)
            {
                await cache.RetrieveAsync<User>(42);
            }

            await afterOperationCallback(CacheOperationType.Retrieve);

            for (var i = processingRange.Start; i < processingRange.End; i++)
            {
                await cache.RemoveAsync<User>(42);
            }

            await afterOperationCallback(CacheOperationType.Remove);
        }

        private static List<User> GenerateUsers(Func<int, (int Id, string Name)> callback)
            => Enumerable.Range(0, 10000000)
                .Select(i =>
                {
                    var userData = callback(i);
                    return new User(userData.Name, userData.Id);
                })
                .ToList();
    }
}