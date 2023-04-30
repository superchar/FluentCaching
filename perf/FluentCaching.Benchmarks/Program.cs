using System;
using System.Globalization;
using System.Reflection;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using Perfolizer.Horology;

namespace FluentCaching.Benchmarks;

public static class Program
{
    internal static void Main()
    {
        BenchmarkRunner.Run(Assembly.GetExecutingAssembly(),
            ManualConfig
                .Create(DefaultConfig.Instance)
                .WithOptions(ConfigOptions.JoinSummary)
                .WithOptions(ConfigOptions.DisableLogFile)
                .WithSummaryStyle(new SummaryStyle(CultureInfo.InvariantCulture, false, SizeUnit.KB,
                    TimeUnit.Millisecond)));
        Console.ReadKey();
    }
}