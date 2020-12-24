

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using FluentCaching.Configuration;

namespace FluentCaching.Benchmarks
{
    public class SimpleKeyBenchmark
    {
        private User[] _users;

        private CachingConfiguration _configuration;

        private Dictionary<string, object> _dictionary;

        [Params(1, 10, 100)]
        public int CacheItemsCount { get; set; }

        [GlobalSetup]
        public void GenerateUsersAndConfiguration()
        {
            _users = new User[CacheItemsCount];

            for (var i = 0; i < _users.Length; i++)
            {
                _users[i] = new User {FirstName = $"FirstName{i}", LastName = $"LastName{i}", Id = i};
            }

            _configuration = CachingConfiguration.Create()
                .SetImplementation(new DictionaryImplementation())
                .For<User>(_ => _.UseAsKey("user").CombinedWith(u => u.Id)
                    .And().WithTtlOf(5).Seconds
                    .And().SlidingExpiration());

            _dictionary = new Dictionary<string, object>();
        }


        [Benchmark(Baseline = true)]
        public void AddAndRetrieveFromDictionary()
        {
            foreach (var user in _users)
            {
                var key = $"user{user.Id}";

                _dictionary[key] = user;

                var retrievedUser = _dictionary[key];
            }
        }

        [Benchmark]
        public async Task CacheAndRetrieve()
        {
            foreach (var user in _users)
            {
                await user.CacheAsync(_configuration);

                var id = user.Id;

                await id.RetrieveAsync<User>(_configuration);
            }
        }
    }
}
