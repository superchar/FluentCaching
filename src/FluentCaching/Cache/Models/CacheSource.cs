namespace FluentCaching.Cache.Models;

public struct CacheSource<T>
    where T : class
{
    public static CacheSource<T> Null = new(null, null);

    public CacheSource(object objectKey) : this(objectKey, null)
    {
    }
    
    public CacheSource(string stringKey) : this(null, stringKey)
    {
    }
    
    private CacheSource(object objectKey, string stringKey)
    { 
        ObjectKey = objectKey;
        StringKey = stringKey;
    }

    public void Deconstruct(out string stringKey, out object objectKey)
    {
        stringKey = StringKey;
        objectKey = ObjectKey;
    }
    
    public object ObjectKey { get; }

    public string StringKey { get; }
}