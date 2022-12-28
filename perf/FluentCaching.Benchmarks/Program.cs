using System;
using System.Threading.Tasks;

namespace FluentCaching.Benchmarks
{
    internal class Program
    {
        internal static async Task Main(string[] args)
        {
            var benchmark = new ComplexKeyBenchmark();
            benchmark.CacheItemsCount = 100;
            benchmark.GenerateUsersAndConfiguration();
            await benchmark.CacheAndRetrieve();
            Console.WriteLine("Started delay");
            await Task.Delay(30000);
            await benchmark.CacheAndRetrieve();
            Console.WriteLine("Finished delay");
            await Task.Delay(30000);
        }
    }
}
