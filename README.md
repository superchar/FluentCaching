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

