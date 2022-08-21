using System;
using System.Linq.Expressions;

namespace FluentCaching.Keys.Builders
{
    internal interface IKeyBuilder
    {
        string BuildFromStaticKey();

        string BuildFromScalarKey(object scalarKey);

        string BuildFromComplexKey(object complexKey);
        
        string BuildFromCachedObject(object cachedObject);
        
        void AppendStatic<TValue>(TValue value);

        void AppendExpression<T, TValue>(Expression<Func<T, TValue>> valueGetter);
        
    }
}
