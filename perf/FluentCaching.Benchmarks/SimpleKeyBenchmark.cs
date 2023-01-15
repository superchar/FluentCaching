using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using FluentCaching.Configuration.PolicyBuilders;
using FluentCaching.Configuration.PolicyBuilders.Keys;

namespace FluentCaching.Benchmarks;

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
        .And().SetExpirationTimeoutTo(5).Seconds
        .With().SlidingExpiration();
}