# FluentCaching
Fluent API for object caching 

*Nothing works yet, currently designing an API*


```csharp
// Use fluent api for objects caching
var user = await _userService.GetUserByIdAsync(42);

await user.UseAsKey("user").CombinedWith(u => u.Id)
.And().WithTtlOf(2).Minutes.And(10).Seconds
.And().SlidingExpiration()
.CacheAsync();

// Or just define caching policies in configuration 
CachingConfiguration.Instance.ForType<User>(builder =>
builder.UseAsKey("user").CombinedWith(u => u.Id)
.And().WithTtlOf(2).Minutes.And(10).Seconds
.And().SlidingExpiration());

// And just use it :)
var user = _userService.GetUserById(42);

await user.CacheAsync();
