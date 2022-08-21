using FluentCaching.Cache;
using FluentCaching.Cache.Builders;
using FluentCaching.Memory;

namespace FluentCaching.Samples.Console 
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var cache = BuildCache();
            
            for (var i = 0; i < 20; i++)
            {
                var factorial = await FactorialAsync(i, cache);
                System.Console.WriteLine($"Factorial of {i} is {factorial}");
            }

            System.Console.ReadKey();
        }

        private static async Task<long> FactorialAsync(int factorial, ICache cache)
        {
            var factorialSource = await cache.RetrieveAsync<FactorialSource>(factorial);
            if (factorialSource != null)
            {
                return factorialSource.Result;
            }

            if (factorial is 0 or 1)
            {
                return 1;
            }

            var result = factorial * await FactorialAsync(factorial - 1, cache);
            factorialSource = new FactorialSource(factorial, result);
            await cache.CacheAsync(factorialSource);
            
            return result;
        }
        
        private static ICache BuildCache() =>
            new CacheBuilder()
                .For<FactorialSource>(_
                    => _.UseAsKey(s => $"Factorial:{s.Factorial}")
                        .And().WithInfiniteTtl()
                        .And().WithInMemoryCache())
                .Build();
    }

    public record FactorialSource(int Factorial, long Result);
}