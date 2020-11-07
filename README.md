# FluentCaching
Fluent API for object caching 

```csharp
    var retrievedUser = new { Name = "My user", Id = 42 }; // Retrieve object from data store

    await retrievedUser.Cache() // Cache it with Fluent API
    .UseAsKey(user => user.Id)
    .WithTtlOf(2).Minutes.And(5).Seconds
    .And().SlidingExpiration()
    .StoreAsync();
    
