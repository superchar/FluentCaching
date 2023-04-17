using FluentCaching.Cache;
using FluentCaching.Cache.Builders;
using FluentCaching.Memory;

namespace FluentCaching.Samples.Console;

public static class Program
{
    public static async Task Main()
    {
        var cache = BuildCache();

        for (var i = 0; i < 90; i++)
        {
            var fibonacciNumber = await FibonacciAsync(i, cache);
            System.Console.WriteLine($"Fibonacci of {i} is {fibonacciNumber}");
        }

        System.Console.ReadKey();
    }

    private static async Task<long> FibonacciAsync(int number, ICache cache)
    {
        var fibonacciSource = await cache.RetrieveAsync<FibonacciSource>(number);
        if (fibonacciSource != null)
        {
            return fibonacciSource.Result;
        }

        if (number < 2)
        {
            return number;
        }

        var result = await FibonacciAsync(number - 1, cache) + await FibonacciAsync(number - 2, cache);
        fibonacciSource = new FibonacciSource(number, result);
        await cache.CacheAsync(fibonacciSource);

        return result;
    }

    private static ICache BuildCache() =>
        new CacheBuilder()
            .For<FibonacciSource>(_
                => _.UseAsKey(s => $"Fibonacci:{s.Number}")
                    .And().SetInfiniteExpirationTimeout()
                    .And().StoreInMemory())
            .Build();
}

public record FibonacciSource(int Number, long Result);