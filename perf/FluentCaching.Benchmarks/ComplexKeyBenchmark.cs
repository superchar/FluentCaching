using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using FluentCaching.Configuration.PolicyBuilders;
using FluentCaching.Configuration.PolicyBuilders.Keys;

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

        protected override AndPolicyBuilder<CacheImplementationPolicyBuilder> Configure(
            CachingKeyPolicyBuilder<User> policyBuilder) =>
            policyBuilder.UseAsKey("user")
                .CombinedWith(_ => _.Id)
                .CombinedWith(_ => _.FirstName)
                .CombinedWith(_ => _.LastName)
                .And().SetExpirationTimeoutTo(5).Seconds
                .With().SlidingExpiration();

        protected override string GetDictionaryKey(User user) => $"user{user.Id}{user.FirstName}{user.LastName}";
    }
}
