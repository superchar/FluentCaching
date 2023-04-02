using System;
using System.Linq.Expressions;

namespace FluentCaching.Keys.Builders;

internal interface IKeyBuilder
{
    string BuildFromStaticKey<TEntity>();

    string BuildFromScalarKey(object scalarKey);

    string BuildFromComplexKey(object complexKey);
        
    string BuildFromCachedObject(object cachedObject);
        
    void AppendStatic<TEntity, TValue>(TValue value);

    void AppendExpression<TEntity, TValue>(Expression<Func<TEntity, TValue>> valueGetter);
        
}