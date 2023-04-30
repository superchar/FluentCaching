using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using FluentCaching.Benchmarks.Models;
using FluentCaching.Configuration.PolicyBuilders;
using FluentCaching.Configuration.PolicyBuilders.Keys;

namespace FluentCaching.Benchmarks.SimpleKey;

public class SimpleKeyRetrieveBenchmark : BaseBenchmark
{
    public override async Task GlobalSetup()
    {
        await base.GlobalSetup();
        await CacheAllUsers();
    }

    [Benchmark]
    public async Task RetrieveWithSimpleKey()
    {
        foreach (var user in Users)
        {
            await Cache.RetrieveAsync<User>(user.Id);
        }
    }

    protected override CacheImplementationPolicyBuilder Configure(
        CachingKeyPolicyBuilder<User> policyBuilder) => ConfigureSimpleKey(policyBuilder);
}