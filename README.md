# FluentCaching
FluentCaching library provides abstraction layer over caching implementation (memory, Redis, etc.) with really small overhead (check benchamrks).
Instead of writing boilerplate code to support caching, just configure caching policy for object using fluent api and use convinient extensions methods to manipulate caching abstraction.  

*Next goals are to prepare examples and first fully working version (memory cache)*

**Configure caching policy by entity**
```csharp
CachingConfiguration.Instance
.For<User>(u => u.UseAsKey("user").CombinedWith(u => u.Id)
.And().WithTtlOf(2).Minutes.And(10).Seconds
.And().SlidingExpiration());
```
**Add object to cache**
```csharp
var user = _userService.GetUserById(42);

await user.CacheAsync();
```

**Retrieve object from cache**
```csharp
var userId = 42;

await userId.RetrieveAsync<User>();

```

**Remove object from cache**
```csharp
var userId = 42;

await userId.RemoveAsync<User>();

```

**Use get or add cache operation**
```csharp
var userId = 42;

var result = await key.RetrieveAsync<User>()
                .Or()
                .CacheAsync(() => _userService.GetUserById(userId))
```

**Multi property configuration is supported with the same set of features**
```csharp
CachingConfiguration.Instance
.For<User>(u => u.UseAsKey(u => u.FirstName).CombinedWith(u => u.LastName)
.And().WithTtlOf(2).Minutes.And(10).Seconds
.And().SlidingExpiration());

var userKey = new {FirstName = "John", LastName = "Doe"}; // may be any class with corresponding properties
await userKey.RetrieveAsync<User>();
```

**Different cache implementations for different entities may be supported**
```csharp
CachingConfiguration.Instance
.For<User>(u => u.UseAsKey(u => u.FirstName).CombinedWith(u => u.LastName)
.And().WithTtlOf(2).Minutes.And(10).Seconds
.And().SlidingExpiration().WithInMemoryCache());

CachingConfiguration.Instance
.For<Order>(o => o.UseAsKey(o => o.Date).CombinedWith("order")
.And().WithTtlOf(5).Minutes.And().SlidingExpiration().WithRedisCache());

```

