using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using FluentCaching.PolicyBuilders;
using FluentCaching.PolicyBuilders.Keys;

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

        protected override AndPolicyBuilder<CacheImplementationPolicyBuilder> Configure(CachingKeyPolicyBuilder<User> policyBuilder) => policyBuilder
            .UseAsKey("user").CombinedWith(u => u.Id)
            .And().WithTtlOf(5).Seconds
            .And().SlidingExpiration();

        protected override string GetDictionaryKey(User user) => $"user{user.Id}";
    }
}
