namespace FluentCaching.Cache.Models;

public struct CacheSource<T>
    where T : class
{
    public static readonly CacheSource<T> Static = new(null, CacheSourceType.Static);

    private CacheSource(object key, CacheSourceType cacheSourceType)
    { 
        Key = key;
        CacheSourceType = cacheSourceType;
    }

    public static CacheSource<T> CreateScalar(object key) => new(key, CacheSourceType.Scalar);
    
    public static CacheSource<T> CreateComplex(object key) => new(key, CacheSourceType.Complex);

    public object Key { get; }
    
    public CacheSourceType CacheSourceType { get; }
}