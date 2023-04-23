using System;

namespace FluentCaching.Cache.Models;

public struct CacheSource<TEntity>
    where TEntity : class
{
    private static readonly CacheSource<TEntity> Static = new(new object(), CacheSourceType.Static);

    private CacheSource(object key, CacheSourceType cacheSourceType)
    {
        Key = key;
        CacheSourceType = cacheSourceType;
    }

    public static CacheSource<TEntity> Create(object? key)
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

        return new CacheSource<TEntity>(key, isScalarType ? CacheSourceType.Scalar : CacheSourceType.Complex);
    }

    public object Key { get; }

    public CacheSourceType CacheSourceType { get; }
}