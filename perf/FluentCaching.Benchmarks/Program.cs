using System;
using BenchmarkDotNet.Running;

namespace FluentCaching.Benchmarks;

public static class Program
{
    internal static void Main()
    {
        BenchmarkRunner.Run<SimpleKeyBenchmark>();
        Console.ReadKey();
    }
}