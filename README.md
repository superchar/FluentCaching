<img src="https://raw.githubusercontent.com/superchar/FluentCaching/main/logo.png" align='center' alt="Fluent caching"> 
FluentCaching library provides abstraction layer over caching implementation (memory, Redis, etc.) with really small overhead (check benchmarks).
Instead of writing boilerplate code to support caching, just configure caching policy for object using fluent api and use cache object to manipulate caching abstraction. 
<br/><br/>The core library is written in plain C# with no external dependencies. 
The nearest plan is to prepare the first release version. 

<h1>Code samples</h1>

Configure caching policy by entity and build cache object
```csharp

var cache = new CacheBuilder()
.For<User>(u => u.UseAsKey("user").CombinedWith(u => u.Id) // alternatively UseAsKey(u => $"user{u.Id}")
                .And().SetExpirationTimeoutTo(2).Minutes.And(10).Seconds
                .With().SlidingExpiration())
.Build();
```
Add object to cache
```csharp
var user = _userService.GetUserById(42);

await cache.CacheAsync(user);
```

Retrieve object from cache
```csharp
var userId = 42;

await cache.RetrieveAsync<User>(userId);

```

Remove object from cache
```csharp
var userId = 42;

await cache.RemoveAsync<User>(userId);

```

Multi property configuration is supported with the same set of features
```csharp
var cache = new CacheBuilder()
.For<User>(u => u.UseAsKey(u => u.FirstName).CombinedWith(u => u.LastName) // alternatively UseAsKey(u => u.FirstName + u.LastName)
                .And().SetExpirationTimeoutTo(2).Minutes.And(10).Seconds
                .With().SlidingExpiration())
.Build();

var userKey = new {FirstName = "John", LastName = "Doe"}; // may be any class or struct with corresponding properties
await cache.RetrieveAsync<User>(userKey);
```

Different cache implementations for different entities are supported
```csharp
var cache = new CacheBuilder()
.For<User>(u => u.UseAsKey(u => u.FirstName).CombinedWith(u => u.LastName)
                .And().SetExpirationTimeoutTo(2).Minutes.And(10).Seconds
                .With().AbsoluteExpiration().And().StoreInMemory())
.For<Order>(o => o.UseAsKey(o => o.Date).CombinedWith("order")
                .And().SetInfiniteExpirationTimeout()
                .And().StoreInDistributedCache())
.Build();

```

<h1>Current benchmarks</h1>

``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.19045
Intel Core i5-9300H CPU 2.40GHz, 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=7.0.202
  [Host]     : .NET Core 6.0.9 (CoreCLR 6.0.922.41905, CoreFX 6.0.922.41905), X64 RyuJIT
  DefaultJob : .NET Core 6.0.9 (CoreCLR 6.0.922.41905, CoreFX 6.0.922.41905), X64 RyuJIT


```
|                 Method | CacheItemsCount |             Mean |           Error |          StdDev |           Median |      Gen 0 |     Gen 1 | Gen 2 |  Allocated |
|----------------------- |---------------- |-----------------:|----------------:|----------------:|-----------------:|-----------:|----------:|------:|-----------:|
|    **CacheWithComplexKey** |             **100** |      **86,697.6 ns** |     **4,644.26 ns** |    **13,399.76 ns** |      **79,066.8 ns** |    **12.8174** |    **1.2207** |     **-** |    **54000 B** |
| RetrieveWithComplexKey |             100 |      92,381.4 ns |     1,116.25 ns |     1,044.15 ns |      92,080.0 ns |    11.4746 |         - |     - |    48400 B |
|     CacheWithSimpleKey |             100 |      72,232.4 ns |     1,282.10 ns |     1,199.28 ns |      72,293.2 ns |    11.7188 |         - |     - |    49281 B |
|  RetrieveWithSimpleKey |             100 |      60,104.3 ns |       257.11 ns |       227.92 ns |      60,050.5 ns |     9.4604 |         - |     - |    39680 B |
|    **CacheWithComplexKey** |            **1000** |     **949,961.4 ns** |     **9,927.90 ns** |     **8,290.25 ns** |     **950,850.3 ns** |   **101.5625** |   **50.7813** |     **-** |   **545674 B** |
| RetrieveWithComplexKey |            1000 |     933,933.4 ns |     6,705.25 ns |     6,272.09 ns |     931,863.2 ns |   116.2109 |         - |     - |   487601 B |
|     CacheWithSimpleKey |            1000 |     789,936.3 ns |    10,507.35 ns |     9,314.50 ns |     788,144.9 ns |   111.3281 |   37.1094 |     - |   495682 B |
|  RetrieveWithSimpleKey |            1000 |     648,466.9 ns |    11,724.92 ns |     9,790.84 ns |     650,408.2 ns |    94.7266 |         - |     - |   399681 B |
|    **CacheWithComplexKey** |           **10000** |  **14,768,348.6 ns** |   **529,612.36 ns** | **1,553,261.37 ns** |  **14,398,479.7 ns** |   **875.0000** |  **437.5000** |     **-** |  **5511620 B** |
| RetrieveWithComplexKey |           10000 |   9,940,724.9 ns |   128,103.67 ns |   119,828.25 ns |   9,921,268.8 ns |  1171.8750 |         - |     - |  4951624 B |
|     CacheWithSimpleKey |           10000 |  11,745,810.1 ns |   128,303.11 ns |   120,014.81 ns |  11,760,548.4 ns |   781.2500 |  390.6250 |     - |  4959700 B |
|  RetrieveWithSimpleKey |           10000 |   7,580,744.7 ns |   230,036.37 ns |   656,306.71 ns |   7,393,207.8 ns |   953.1250 |         - |     - |  3999690 B |
|    **CacheWithComplexKey** |          **100000** | **216,391,507.9 ns** | **4,180,092.27 ns** | **9,604,457.14 ns** | **212,963,600.0 ns** |  **8000.0000** | **3000.0000** |     **-** | **55912896 B** |
| RetrieveWithComplexKey |          100000 | 103,738,083.3 ns | 1,397,357.03 ns | 1,307,088.63 ns | 103,625,016.7 ns | 12000.0000 |         - |     - | 50311941 B |
|     CacheWithSimpleKey |          100000 | 193,362,800.0 ns | 1,511,066.87 ns | 1,339,521.42 ns | 193,063,900.0 ns |  8000.0000 | 3000.0000 |     - | 50323008 B |
|  RetrieveWithSimpleKey |          100000 |  80,505,008.3 ns | 1,594,134.10 ns | 2,663,439.96 ns |  79,443,908.3 ns |  9666.6667 |         - |     - | 40720257 B |




