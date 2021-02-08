using System;
using BenchmarkDotNet.Running;

namespace FluentCaching.Benchmarks
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<ComplexKeyBenchmark>();

            Console.ReadKey();
        }
    }
}
