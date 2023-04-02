using System.Collections.Generic;

namespace FluentCaching.Keys.Models;

public struct KeyContext
{
    public static readonly KeyContext Null = new(null, null);

    public static readonly KeyContext Empty = new(new Dictionary<string, object>());

    public KeyContext(object store) : this(store, null)
    {
    }
        
    public KeyContext(Dictionary<string, object> retrieve) : this(default, retrieve)
    {
    }

    private KeyContext(object store, Dictionary<string, object> retrieve)
    {
        Store = store;
        Retrieve = retrieve;
    }

    public object Store { get; }

    public Dictionary<string, object> Retrieve { get; }
}