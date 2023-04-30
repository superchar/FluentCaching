using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using FluentCaching.Benchmarks.Models;
using FluentCaching.Configuration.PolicyBuilders;
using FluentCaching.Configuration.PolicyBuilders.Keys;

namespace FluentCaching.Benchmarks.ComplexKey;

public class ComplexKeyRetrieveBenchmark : BaseBenchmark
{
    public override async Task GlobalSetup()
    {
        await base.GlobalSetup();
        await CacheAllUsers();
    }
    
    [Benchmark]
    public async Task RetrieveWithComplexKey()
    {
        foreach (var user in Users)
        {
            var key = new {user.Id, user.LastName, user.FirstName};

            await Cache.RetrieveAsync<User>(key);
        }
    }

    protected override CacheImplementationPolicyBuilder Configure(
        CachingKeyPolicyBuilder<User> policyBuilder) => ConfigureComplexKey(policyBuilder);
}