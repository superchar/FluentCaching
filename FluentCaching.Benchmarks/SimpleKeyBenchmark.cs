using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using FluentCaching.PolicyBuilders.Keys;
using FluentCaching.PolicyBuilders.Ttl;

namespace FluentCaching.Benchmarks
{
    public class SimpleKeyBenchmark : BaseDictionaryCompareBenchmark
    {
        [Benchmark]
        public async Task CacheAndRetrieve()
        {
            foreach (var user in Users)
            {
                await Cache.CacheAsync(user);

                var id = user.Id;

                await Cache.RetrieveAsync<User>(id);
            }
        }

        protected override ExpirationTypeBuilder Configure(CachingKeyBuilder<User> builder) => builder
            .UseAsKey("user").CombinedWith(u => u.Id)
            .And().WithTtlOf(5).Seconds
            .And().SlidingExpiration();

        protected override string GetDictionaryKey(User user) => $"user{user.Id}";
    }
}
