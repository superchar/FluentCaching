using System;
using System.Collections.Generic;
using System.Text;
using BenchmarkDotNet.Attributes;
using FluentCaching.Api;
using FluentCaching.Api.Keys;
using FluentCaching.Configuration;

namespace FluentCaching.Benchmarks
{
    public abstract class BaseDictionaryCompareBenchmark
    {
        private Dictionary<string, object> _dictionary;

        [Params(1, 10, 100)]
        public int CacheItemsCount { get; set; }

        protected User[] Users { get; private set; }

        protected CachingConfiguration Configuration { get; private set; }

        [GlobalSetup]
        public void GenerateUsersAndConfiguration()
        {
            Users = new User[CacheItemsCount];

            for (var i = 0; i < Users.Length; i++)
            {
                Users[i] = new User { FirstName = $"FirstName{i}", LastName = $"LastName{i}", Id = i };
            }

            Configuration = CachingConfiguration.Create()
                .SetImplementation(new DictionaryImplementation())
                .For<User>(Configure);

            _dictionary = new Dictionary<string, object>();
        }

        [Benchmark(Baseline = true)]
        public void AddAndRetrieveFromDictionary()
        {
            foreach (var user in Users)
            {
                var key = $"user{user.Id}";

                _dictionary[key] = user;

                var retrievedUser = _dictionary[key];
            }
        }

        protected abstract ExpirationBuilder Configure(CachingKeyBuilder<User> builder);

        protected abstract string GetDictionaryKey(User user);
    }
}
