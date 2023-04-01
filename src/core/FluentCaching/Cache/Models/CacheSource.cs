using System;

namespace FluentCaching.Cache.Models;

public struct CacheSource<T>
    where T : class
{
    private static readonly CacheSource<T> Static = new(null, CacheSourceType.Static);

    private CacheSource(object key, CacheSourceType cacheSourceType)
    {
        Key = key;
        CacheSourceType = cacheSourceType;
    }

    public static CacheSource<T> Create(object key)
    {
        if (key == null)
        {
            return Static;
        }

        var type = key.GetType();
        var isScalarType = type.IsPrimitive
                           || type == typeof(string)
                           || type == typeof(decimal)
                           || type == typeof(Guid);

        return new CacheSource<T>(key, isScalarType ? CacheSourceType.Scalar : CacheSourceType.Complex);
    }

    public object Key { get; }

    public CacheSourceType CacheSourceType { get; }
}