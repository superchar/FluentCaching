# FluentCaching
Fluent API for object caching 

*Nothing works yet, currently designing an API*

**Configure caching policy by entity**
```csharp
CachingConfiguration.Instance.ForType<User>(builder =>
builder.UseAsKey("user").CombinedWith(u => u.Id)
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

**Or use multi property configuration**
```csharp
CachingConfiguration.Instance.ForType<User>(builder =>
builder.UseAsKey(u => u.FirstName).CombinedWith(u => u.LastName)
.And().WithTtlOf(2).Minutes.And(10).Seconds
.And().SlidingExpiration());

```

**And retrieve it with multi property key**
```csharp
var userKey = new {FirstName = "John", LastName = "Dou"}; // may be any class with corresponding properties
await userKey.RetrieveAsync<User>();
```

