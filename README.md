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
|                 Method | ItemsCount |       Mean |     Error |    StdDev |     Median |     Gen 0 |    Gen 1 | Gen 2 |  Allocated |
|----------------------- |----------- |-----------:|----------:|----------:|-----------:|----------:|---------:|------:|-----------:|
|    **CacheWithComplexKey** |        **100** |  **0.3616 ms** | **0.0455 ms** | **0.1340 ms** |  **0.3616 ms** |         **-** |        **-** |     **-** |   **53.72 KB** |
| RetrieveWithComplexKey |        100 |  0.0924 ms | 0.0018 ms | 0.0027 ms |  0.0919 ms |   11.4746 |        - |     - |   47.27 KB |
|     CacheWithSimpleKey |        100 |  0.0821 ms | 0.0051 ms | 0.0148 ms |  0.0736 ms |   11.7188 |        - |     - |   48.13 KB |
|  RetrieveWithSimpleKey |        100 |  0.0656 ms | 0.0013 ms | 0.0015 ms |  0.0652 ms |    9.4604 |        - |     - |   38.75 KB |
|    **CacheWithComplexKey** |       **1000** |  **1.0152 ms** | **0.0473 ms** | **0.1349 ms** |  **0.9603 ms** |  **101.5625** |  **50.7813** |     **-** |  **530.86 KB** |
| RetrieveWithComplexKey |       1000 |  1.0323 ms | 0.0519 ms | 0.1522 ms |  0.9536 ms |  116.2109 |        - |     - |  476.17 KB |
|     CacheWithSimpleKey |       1000 |  0.9037 ms | 0.0436 ms | 0.1251 ms |  0.8571 ms |  111.3281 |  37.1094 |     - |  484.06 KB |
|  RetrieveWithSimpleKey |       1000 |  0.6686 ms | 0.0088 ms | 0.0078 ms |  0.6677 ms |   94.7266 |        - |     - |  390.32 KB |
|    **CacheWithComplexKey** |      **10000** | **14.1614 ms** | **0.4543 ms** | **1.2437 ms** | **13.6662 ms** |  **875.0000** | **437.5000** |     **-** | **5382.46 KB** |
| RetrieveWithComplexKey |      10000 | 10.6172 ms | 0.5431 ms | 1.5139 ms |  9.8729 ms | 1171.8750 |        - |     - | 4835.57 KB |
|     CacheWithSimpleKey |      10000 | 12.1970 ms | 0.2404 ms | 0.5713 ms | 12.0789 ms |  781.2500 | 390.6250 |     - | 4845.48 KB |
|  RetrieveWithSimpleKey |      10000 |  6.8092 ms | 0.1594 ms | 0.4471 ms |  6.6842 ms |  953.1250 |        - |     - | 3905.95 KB |
