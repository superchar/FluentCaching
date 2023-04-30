using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using FluentCaching.Benchmarks.Models;
using FluentCaching.Cache;
using FluentCaching.Cache.Builders;
using FluentCaching.Configuration.PolicyBuilders;
using FluentCaching.Configuration.PolicyBuilders.Keys;
using FluentCaching.Memory;

namespace FluentCaching.Benchmarks;

[MemoryDiagnoser]
public abstract class BaseBenchmark
{
    [Params(100, 1000, 10000)]
    // ReSharper disable once MemberCanBePrivate.Global
    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public int ItemsCount { get; set; }

    protected User[] Users { get; private set; }

    protected ICache Cache { get; private set; }

    [GlobalSetup]
    public virtual Task GlobalSetup()
    {
        Cache = new CacheBuilder()
            .For<User>(Configure)
            .Build();

        Users = new User[ItemsCount];
        for (var i = 0; i < Users.Length; i++)
        {
            Users[i] = new User { FirstName = $"FirstName{i}", LastName = $"LastName{i}", Id = i };
        }

        return Task.CompletedTask;
    }

    protected static CacheImplementationPolicyBuilder ConfigureSimpleKey(
        CachingKeyPolicyBuilder<User> policyBuilder) =>
        policyBuilder
            .UseAsKey("user").CombinedWith(u => u.Id)
            .And().SetExpirationTimeoutTo(5).Seconds
            .With().SlidingExpiration()
            .And().StoreInMemory();

    protected static CacheImplementationPolicyBuilder ConfigureComplexKey(
        CachingKeyPolicyBuilder<User> policyBuilder) =>
        policyBuilder
            .UseAsKey("user").CombinedWith(_ => _.Id).CombinedWith(_ => _.FirstName)
            .CombinedWith(_ => _.LastName)
            .And().SetExpirationTimeoutTo(5).Seconds
            .With().SlidingExpiration()
            .And().StoreInMemory();


    protected abstract CacheImplementationPolicyBuilder Configure(
        CachingKeyPolicyBuilder<User> policyBuilder);

    protected async Task CacheAllUsers()
    {
        foreach (var user in Users)
        {
            await Cache.CacheAsync(user);
        }
    }
}