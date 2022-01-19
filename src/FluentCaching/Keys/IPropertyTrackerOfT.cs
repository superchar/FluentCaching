using System;
using System.Linq.Expressions;

namespace FluentCaching.Keys
{
    internal interface IPropertyTracker<T> : IPropertyTracker where T : class
    {
        string GetStoreKey(T obj);

        void TrackExpression<TValue>(Expression<Func<T, TValue>> valueGetter);

    }
}
