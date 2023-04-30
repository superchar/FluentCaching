using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using FluentCaching.Benchmarks.Models;
using FluentCaching.Configuration.PolicyBuilders;
using FluentCaching.Configuration.PolicyBuilders.Keys;

namespace FluentCaching.Benchmarks.ComplexKey;

public class ComplexKeyCacheBenchmark : BaseBenchmark
{
    [Benchmark]
    public Task CacheWithComplexKey()
        => CacheAllUsers();

    protected override CacheImplementationPolicyBuilder Configure(CachingKeyPolicyBuilder<User> policyBuilder)
        => ConfigureComplexKey(policyBuilder);
}