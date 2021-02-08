

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using FluentCaching.Api;
using FluentCaching.Api.Keys;
using FluentCaching.Configuration;

namespace FluentCaching.Benchmarks
{
    public class SimpleKeyBenchmark : BaseDictionaryCompareBenchmark
    {
        [Benchmark]
        public async Task CacheAndRetrieve()
        {
            foreach (var user in Users)
            {
                await user.CacheAsync(Configuration);

                var id = user.Id;

                await id.RetrieveAsync<User>(Configuration);
            }
        }

        protected override ExpirationBuilder Configure(CachingKeyBuilder<User> builder) => builder
            .UseAsKey("user").CombinedWith(u => u.Id)
            .And().WithTtlOf(5).Seconds
            .And().SlidingExpiration();


        protected override string GetDictionaryKey(User user) => $"user{user.Id}";
    }
}
