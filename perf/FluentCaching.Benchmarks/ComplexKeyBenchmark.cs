using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using FluentCaching.PolicyBuilders;
using FluentCaching.PolicyBuilders.Keys;
using FluentCaching.PolicyBuilders.Ttl;

namespace FluentCaching.Benchmarks
{
    public class ComplexKeyBenchmark : BaseDictionaryCompareBenchmark
    {
        [Benchmark]
        public async Task CacheAndRetrieve()
        {
            foreach (var user in Users)
            {
                await Cache.CacheAsync(user);

                var key = new {user.Id, user.LastName, user.FirstName};

                await Cache.RetrieveAsync<User>(key);
            }
        }

        protected override AndBuilder<CacheImplementationBuilder> Configure(CachingKeyBuilder<User> builder) =>
            builder.UseAsKey("user")
                .CombinedWith(_ => _.Id)
                .CombinedWith(_ => _.FirstName)
                .CombinedWith(_ => _.LastName)
                .And().WithTtlOf(5).Seconds
                .And().SlidingExpiration();

        protected override string GetDictionaryKey(User user) => $"user{user.Id}{user.FirstName}{user.LastName}";
    }
}
