using System;
using System.Threading.Tasks;
using BenchmarkDotNet.Running;

namespace FluentCaching.Benchmarks
{
    internal class Program
    {
        internal static async Task Main(string[] args)
        {
            BenchmarkRunner.Run<SimpleKeyBenchmark>();
            Console.ReadKey();
        }
    }
}
