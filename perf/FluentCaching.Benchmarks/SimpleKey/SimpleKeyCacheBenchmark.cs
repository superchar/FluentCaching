using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using FluentCaching.Benchmarks.Models;
using FluentCaching.Configuration.PolicyBuilders;
using FluentCaching.Configuration.PolicyBuilders.Keys;

namespace FluentCaching.Benchmarks.SimpleKey;

public class SimpleKeyCacheBenchmark : BaseBenchmark
{
    [Benchmark]
    public Task CacheWithSimpleKey()
        => CacheAllUsers();

    protected override CacheImplementationPolicyBuilder Configure(CachingKeyPolicyBuilder<User> policyBuilder)
        => ConfigureSimpleKey(policyBuilder);
}