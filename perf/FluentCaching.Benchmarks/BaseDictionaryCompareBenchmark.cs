using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using FluentCaching.Cache;
using FluentCaching.Cache.Builders;
using FluentCaching.Configuration.PolicyBuilders;
using FluentCaching.Configuration.PolicyBuilders.Keys;

namespace FluentCaching.Benchmarks;

[MemoryDiagnoser]
public abstract class BaseDictionaryCompareBenchmark
{
    private Dictionary<string, object> _dictionary;

    [Params(1, 10, 100, 1000, 10000)]
    // ReSharper disable once MemberCanBePrivate.Global
    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public int CacheItemsCount { get; set; }

    protected User[] Users { get; private set; }

    internal ICache Cache { get; private set; }

    [GlobalSetup]
    public void GenerateUsersAndConfiguration()
    {
        Users = new User[CacheItemsCount];

        for (var i = 0; i < Users.Length; i++)
        {
            Users[i] = new User { FirstName = $"FirstName{i}", LastName = $"LastName{i}", Id = i };
        }

        Cache = new CacheBuilder()
            .SetGenericCache(new DictionaryImplementation())
            .For<User>(Configure)
            .Build();

        _dictionary = new Dictionary<string, object>(CacheItemsCount);
    }

    [Benchmark(Baseline = true)]
    public void AddAndRetrieveFromDictionary()
    {
        foreach (var user in Users)
        {
            var key = $"user{user.Id}";

            _dictionary[key] = user;

            _ = _dictionary[key];
        }
    }

    protected abstract AndPolicyBuilder<CacheImplementationPolicyBuilder> Configure(CachingKeyPolicyBuilder<User> policyBuilder);
}