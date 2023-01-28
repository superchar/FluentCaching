using System;

namespace FluentCaching.Keys.Models;

internal struct PropertyAccessor
{
    public PropertyAccessor(string name, Func<object, object> get)
    {
        Name = name;
        Get = get;
    }

    public string Name { get; }

    public Func<object, object> Get { get; }
}