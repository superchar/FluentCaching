# FluentCaching
FluentCaching library provides abstraction layer over caching implementation (memory, Redis, etc.) with really small overhead (check benchmarks).
Instead of writing boilerplate code to support caching, just configure caching policy for object using fluent api and use cache object to manipulate caching abstraction.  

**Configure caching policy by entity and build cache object**
```csharp

var cache = new CacheBuilder()
.For<User>(u => u.UseAsKey("user").CombinedWith(u => u.Id) // alternatively UseAsKey(u => $"user{u.Id}")
                .And().WithTtlOf(2).Minutes.And(10).Seconds
                .And().SlidingExpiration())
.Build();
```
**Add object to cache**
```csharp
var user = _userService.GetUserById(42);

await cache.CacheAsync(user);
```

**Retrieve object from cache**
```csharp
var userId = 42;

await cache.RetrieveAsync<User>(userId);

```

**Remove object from cache**
```csharp
var userId = 42;

await cache.RemoveAsync<User>(userId);

```

**Multi property configuration is supported with the same set of features**
```csharp
var cache = new CacheBuilder()
.For<User>(u => u.UseAsKey(u => u.FirstName).CombinedWith(u => u.LastName) // alternatively UseAsKey(u => u.FirstName + u.LastName)
                .And().WithTtlOf(2).Minutes.And(10).Seconds
                .And().SlidingExpiration())
.Build();

var userKey = new {FirstName = "John", LastName = "Doe"}; // may be any class with corresponding properties
await cache.RetrieveAsync<User>(userKey);
```

**Different cache implementations for different entities are supported**
```csharp
var cache = new CacheBuilder()
.For<User>(u => u.UseAsKey(u => u.FirstName).CombinedWith(u => u.LastName)
                .And().WithTtlOf(2).Minutes.And(10).Seconds
                .And().SlidingExpiration().UseInMemoryCache())
.For<Order>(o => o.UseAsKey(o => o.Date).CombinedWith("order")
                .And().WithTtlOf(5).Minutes
                .And().SlidingExpiration().UseDistributedCache())
.Build();

```

