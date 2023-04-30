using System;
using System.Reflection;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;

namespace FluentCaching.Benchmarks;

public static class Program
{
    internal static void Main()
    {
        BenchmarkRunner.Run(Assembly.GetExecutingAssembly(),
            ManualConfig
                .Create(DefaultConfig.Instance)
                .WithOptions(ConfigOptions.JoinSummary)
                .WithOptions(ConfigOptions.DisableLogFile));
        Console.ReadKey();
    }
}