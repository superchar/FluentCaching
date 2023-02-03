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
            await RunForegroundСacheOperations(autoResetEvent, cache, users, backgroudTask, id, CreateScalarKey);
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
            await RunForegroundСacheOperations(autoResetEvent, cache, users, backgroudTask, CreateScalarKey(targetUser),
                CreateScalarKey);
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
            await RunForegroundСacheOperations(autoResetEvent, cache, users, backgroudTask, key, CreateComplexKey);
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
            await RunForegroundСacheOperations(autoResetEvent, cache, users, backgroudTask, CreateComplexKey(targetUser), CreateComplexKey);
        }

        private static Task RunBackgroundСacheOperations(WaitHandle autoResetEvent, 
            ICache cache,
            User[] users,
            Func<User, object> createKeyFunc)
            => Task.Run(() => RunCacheOperations(users[..(users.Length / 2)], cache, createKeyFunc, 
                    _ => Task.FromResult(autoResetEvent.WaitOne())));

        private static Task RunForegroundСacheOperations(
            EventWaitHandle autoResetEvent,
            ICache cache,
            User[] users,
            Task backgroudTask,
            object assertionKey,
            Func<User, object> createKeyFunc)
            => RunCacheOperations(users[(users.Length / 2)..], cache, createKeyFunc,
                afterCacheCallback: () =>
                {
                    autoResetEvent.Set();
                    return Task.CompletedTask;
                },
                afterRetrieveCallback: async () =>
                {
                    var user = await cache.RetrieveAsync<User>(assertionKey);
                    user.Should().NotBeNull();
                    autoResetEvent.Set();
                },
                afterRemoveCallback: async () =>
                {
                    autoResetEvent.Set();
                    await backgroudTask;
                    var removedUser = await cache.RetrieveAsync<User>(assertionKey);
                    removedUser.Should().BeNull();
                });

        private static Task RunCacheOperations(User[] users,
            ICache cache,
            Func<User, object> createKeyFunc,
            Func<Task> afterCacheCallback,
            Func<Task> afterRetrieveCallback,
            Func<Task> afterRemoveCallback)
            => RunCacheOperations(users, cache, createKeyFunc,
                operationType => operationType switch
                {
                    CacheOperationType.Cache => afterCacheCallback(),
                    CacheOperationType.Retrieve => afterRetrieveCallback(),
                    CacheOperationType.Remove => afterRemoveCallback(),
                    _ => throw new ArgumentOutOfRangeException(nameof(operationType), operationType, null)
                });
        
        private static async Task RunCacheOperations(User[] users,
            ICache cache,
            Func<User, object> createKeyFunc,
            Func<CacheOperationType, Task> afterOperationCallback)
        {
            foreach (var user in users)
            {
                await cache.CacheAsync(user);
            }
            await afterOperationCallback(CacheOperationType.Cache);
            
            foreach (var user in users)
            {
                await cache.RetrieveAsync<User>(createKeyFunc(user));
            }
            await afterOperationCallback(CacheOperationType.Retrieve);

            foreach (var user in users)
            {
                await cache.RemoveAsync<User>(createKeyFunc(user));
            }
            await afterOperationCallback(CacheOperationType.Remove);
        }

        private static User[] GenerateUsers(Func<int, (int Id, string Name)> callback)
            => Enumerable.Range(0, 10000000)
                .Select(i =>
                {
                    var userData = callback(i);
                    return new User(userData.Name, userData.Id);
                })
                .ToArray();
        
        private static object CreateScalarKey(User user) => user.Id;

        private static object CreateComplexKey(User user) => new { user.Id, user.Name };
    }
}