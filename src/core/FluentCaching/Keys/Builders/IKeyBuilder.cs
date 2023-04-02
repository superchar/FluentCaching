using System;
using System.Linq.Expressions;

namespace FluentCaching.Keys.Builders;

internal interface IKeyBuilder
{
    string BuildFromStaticKey<TEntity>();

    string BuildFromScalarKey<TEntity>(object scalarKey);

    string BuildFromComplexKey<TEntity>(object complexKey);
        
    string BuildFromCachedObject(object cachedObject);
        
    void AppendStatic<T, TValue>(TValue value);

    void AppendExpression<T, TValue>(Expression<Func<T, TValue>> valueGetter);
        
}