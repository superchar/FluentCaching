using System;
using BenchmarkDotNet.Running;

namespace FluentCaching.Benchmarks
{
    internal class Program
    {
        internal static void Main(string[] args)
        {
            BenchmarkRunner.Run(typeof(Program).Assembly);
            Console.ReadKey();
        }
    }
}
