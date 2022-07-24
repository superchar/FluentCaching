using System;
using System.Linq.Expressions;

namespace FluentCaching.Keys.Builders
{
    internal interface IKeyBuilder<T> : IKeyBuilder where T : class
    {
        string BuildFromCachedObject(T cachedObject);

        void AppendExpression<TValue>(Expression<Func<T, TValue>> valueGetter);

    }
}
