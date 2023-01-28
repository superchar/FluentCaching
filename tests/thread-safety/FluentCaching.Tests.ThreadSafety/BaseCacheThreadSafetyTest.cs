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
        public async Task ScalarKeyTypeWithTheSameKey()
        {
            const int id = 42;
            var cache = new CacheBuilder()
                .For<User>(_ => _.UseAsKey(u => u.Id).And()
                    .SetInfiniteExpirationTimeout().And()
                    .StoreIn(CacheImplementation))
                .Build();

            var users = GenerateUsers(_ => (id, "Some Name"));
            var autoResetEvent = new AutoResetEvent(false);
            var backgroudTask = RunBackgroundСacheOperations(autoResetEvent, cache, users, CreateScalarKey);

            await RunCacheOperations(users, cache, (users.Count / 2, users.Count), CreateScalarKey,
                afterCacheCallback: () =>
                {
                    autoResetEvent.Set();
                    return Task.CompletedTask;
                },
                afterRetrieveCallback: async () =>
                {
                    var user = await cache.RetrieveAsync<User>(id);
                    user.Should().NotBeNull();
                    autoResetEvent.Set();
                },
                afterRemoveCallback: async () =>
                {
                    autoResetEvent.Set();
                    await backgroudTask;
                    var removedUser = await cache.RetrieveAsync<User>(id);
                    removedUser.Should().BeNull();
                });
        }
        
        [Fact]
        public async Task ScalarKeyTypeWithTheDifferentKey()
        {
            var cache = new CacheBuilder()
                .For<User>(_ => _.UseAsKey(u => u.Id).And()
                    .SetInfiniteExpirationTimeout().And()
                    .StoreIn(CacheImplementation))
                .Build();

            var users = GenerateUsers(index => (index, $"Some Name {index}"));
            var targetUser = users.GetRandomItem();
            var autoResetEvent = new AutoResetEvent(false);

            var backgroudTask = RunBackgroundСacheOperations(autoResetEvent, cache, users, CreateScalarKey);

            await RunCacheOperations(users, cache, (users.Count / 2, users.Count), CreateScalarKey,
                afterCacheCallback: () =>
                {
                    autoResetEvent.Set();
                    return Task.CompletedTask;
                },
                afterRetrieveCallback: async () =>
                {
                    var user = await cache.RetrieveAsync<User>(CreateScalarKey(targetUser));
                    user.Should().NotBeNull();
                    autoResetEvent.Set();
                },
                afterRemoveCallback: async () =>
                {
                    autoResetEvent.Set();
                    await backgroudTask;
                    var removedUser = await cache.RetrieveAsync<User>(targetUser.Id);
                    removedUser.Should().BeNull();
                });
        }
        
        [Fact]
        public async Task ComplexKeyTypeWithTheSameKey()
        {
            var key = new {Id = 42, Name = "Some Name"};
            var cache = new CacheBuilder()
                .For<User>(_ => _.UseAsKey(u => $"{u.Id}-{u.Name}").And()
                    .SetInfiniteExpirationTimeout().And()
                    .StoreIn(CacheImplementation))
                .Build();

            var users = GenerateUsers(_ => (key.Id, key.Name));
            var autoResetEvent = new AutoResetEvent(false);

            var backgroudTask = RunBackgroundСacheOperations(autoResetEvent, cache, users, CreateComplexKey);

            await RunCacheOperations(users, cache, (users.Count / 2, users.Count), CreateComplexKey,
                afterCacheCallback: () =>
                {
                    autoResetEvent.Set();
                    return Task.CompletedTask;
                },
                afterRetrieveCallback: async () =>
                {
                    var user = await cache.RetrieveAsync<User>(key);
                    user.Should().NotBeNull();
                    autoResetEvent.Set();
                },
                afterRemoveCallback: async () =>
                {
                    autoResetEvent.Set();
                    await backgroudTask;
                    var removedUser = await cache.RetrieveAsync<User>(key);
                    removedUser.Should().BeNull();
                });
        }
        
        [Fact]
        public async Task ComplexKeyTypeWithTheDifferentKey()
        {
            var cache = new CacheBuilder()
                .For<User>(_ => _.UseAsKey(u => $"{u.Id}-{u.Name}").And()
                    .SetInfiniteExpirationTimeout().And()
                    .StoreIn(CacheImplementation))
                .Build();

            var users = GenerateUsers(index => (index, $"Some name {index}"));
            var targetUser = users.GetRandomItem();
            var autoResetEvent = new AutoResetEvent(false);

            var backgroudTask = RunBackgroundСacheOperations(autoResetEvent, cache, users, CreateComplexKey);

            await RunCacheOperations(users, cache, (users.Count / 2, users.Count), CreateComplexKey,
                afterCacheCallback: () =>
                {
                    autoResetEvent.Set();
                    return Task.CompletedTask;
                },
                afterRetrieveCallback: async () =>
                {
                    var user = await cache.RetrieveAsync<User>(CreateComplexKey(targetUser));
                    user.Should().NotBeNull();
                    autoResetEvent.Set();
                },
                afterRemoveCallback: async () =>
                {
                    autoResetEvent.Set();
                    await backgroudTask;
                    var removedUser = await cache.RetrieveAsync<User>(CreateComplexKey(targetUser));
                    removedUser.Should().BeNull();
                });
        }

        private static object CreateScalarKey(User user) => user.Id;

        private static object CreateComplexKey(User user) => new { user.Id, user.Name };
        
        private static Task RunBackgroundСacheOperations(WaitHandle autoResetEvent, 
            ICache cache,
            IReadOnlyList<User> users,
            Func<User, object> createKeyFunc)
            => Task.Run(() => RunCacheOperations(users, cache, (0, users.Count / 2), createKeyFunc, 
                    _ => Task.FromResult(autoResetEvent.WaitOne())));

        private static Task RunCacheOperations(IReadOnlyList<User> users,
            ICache cache,
            (int Start, int End ) processingRange,
            Func<User, object> createKeyFunc,
            Func<Task> afterCacheCallback,
            Func<Task> afterRetrieveCallback,
            Func<Task> afterRemoveCallback)
            => RunCacheOperations(users, cache, processingRange, createKeyFunc,
                operationType => operationType switch
                {
                    CacheOperationType.Cache => afterCacheCallback(),
                    CacheOperationType.Retrieve => afterRetrieveCallback(),
                    CacheOperationType.Remove => afterRemoveCallback(),
                    _ => throw new ArgumentOutOfRangeException(nameof(operationType), operationType, null)
                });
        
        private static async Task RunCacheOperations(IReadOnlyList<User> users,
            ICache cache,
            (int Start, int End ) processingRange,
            Func<User, object> createKeyFunc,
            Func<CacheOperationType, Task> afterOperationCallback)
        {
            for (var i = processingRange.Start; i < processingRange.End; i++)
            {
                await cache.CacheAsync(users[i]);
            }

            await afterOperationCallback(CacheOperationType.Cache);
            for (var i = processingRange.Start; i < processingRange.End; i++)
            {
                await cache.RetrieveAsync<User>(createKeyFunc(users[i]));
            }

            await afterOperationCallback(CacheOperationType.Retrieve);
            for (var i = processingRange.Start; i < processingRange.End; i++)
            {
                await cache.RemoveAsync<User>(createKeyFunc(users[i]));
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