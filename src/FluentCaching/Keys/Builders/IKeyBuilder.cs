using System;
using System.Linq.Expressions;

namespace FluentCaching.Keys.Builders
{
    internal interface IKeyBuilder
    {
        string BuildFromStringKey(string stringKey);

        string BuildFromObjectKey(object objectKey);

        string BuildFromStaticKey();

        void AppendStatic<TValue>(TValue value);
        
        string BuildFromCachedObject(object cachedObject);

        void AppendExpression<T, TValue>(Expression<Func<T, TValue>> valueGetter);
        
    }
}
