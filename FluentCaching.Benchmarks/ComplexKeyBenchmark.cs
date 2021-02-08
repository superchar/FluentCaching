using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using FluentCaching.Api;
using FluentCaching.Api.Keys;

namespace FluentCaching.Benchmarks
{
    public class ComplexKeyBenchmark : BaseDictionaryCompareBenchmark
    {
        [Benchmark]
        public async Task CacheAndRetrieve()
        {
            foreach (var user in Users)
            {
                await user.CacheAsync(Configuration);

                var key = new {user.Id, user.LastName, user.FirstName};

                await key.RetrieveAsync<User>(Configuration);
            }
        }

        protected override ExpirationBuilder Configure(CachingKeyBuilder<User> builder) =>
            builder.UseAsKey("user")
                .CombinedWith(_ => _.Id)
                .CombinedWith(_ => _.FirstName)
                .CombinedWith(_ => _.LastName)
                .And().WithTtlOf(5).Seconds
                .And().SlidingExpiration();

        protected override string GetDictionaryKey(User user) => $"user{user.Id}{user.FirstName}{user.LastName}";
    }
}
