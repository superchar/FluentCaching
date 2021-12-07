using System;
using System.Linq.Expressions;
using FluentCaching.Keys;

namespace FluentCaching.PolicyBuilders.Keys
{
    public class CachingKeyBuilder<T>
        where T : class
    {
        private static readonly string ClassName = typeof(T).Name;

        private static readonly string ClassFullName = typeof(T).FullName;

        private readonly PropertyTracker<T> _propertyTracker = new PropertyTracker<T>();

        public CombinedCachingKeyBuilder<T> UseAsKey<TValue>(Expression<Func<T, TValue>> valueGetter)
        {
            _propertyTracker.TrackExpression(valueGetter);
            return new CombinedCachingKeyBuilder<T>(_propertyTracker);
        }

        public CombinedCachingKeyBuilder<T> UseAsKey<TValue>(TValue value)
        {
            _propertyTracker.TrackStatic(value);
            return new CombinedCachingKeyBuilder<T>(_propertyTracker);
        }

        public CombinedCachingKeyBuilder<T> UseClassNameAsKey()
        {
            _propertyTracker.TrackStatic(ClassName);
            return new CombinedCachingKeyBuilder<T>(_propertyTracker);
        }

        public CombinedCachingKeyBuilder<T> UseClassFullNameAsKey()
        {
            _propertyTracker.TrackStatic(ClassFullName);
            return new CombinedCachingKeyBuilder<T>(_propertyTracker);
        }
    }
}
