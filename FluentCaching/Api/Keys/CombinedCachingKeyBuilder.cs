using System;
using System.Linq.Expressions;
using FluentCaching.Keys;

namespace FluentCaching.Api.Keys
{
    public class CombinedCachingKeyBuilder<T>
        where T : class
    {
        private static readonly string ClassName = typeof(T).Name;

        private static readonly string ClassFullName = typeof(T).FullName;

        private readonly PropertyTracker<T> _propertyTracker;

        internal CombinedCachingKeyBuilder(PropertyTracker<T> propertyTracker)
        {
            _propertyTracker = propertyTracker;
        }

        public CachingOptionsBuilder And() => new CachingOptionsBuilder(_propertyTracker);

        public CombinedCachingKeyBuilder<T> CombinedWith<TValue>(Expression<Func<T, TValue>> valueGetter)
        {
            _propertyTracker.TrackExpression(valueGetter);
            return this;
        }

        public CombinedCachingKeyBuilder<T> CombinedWith<TValue>(TValue value)
        {
            _propertyTracker.TrackStatic(value);
            return this;
        }

        public CombinedCachingKeyBuilder<T> CombinedWithClassName()
        {
            _propertyTracker.TrackStatic(ClassName);
            return this;
        }

        public CombinedCachingKeyBuilder<T> CombinedWithClassFullName()
        {
            _propertyTracker.TrackStatic(ClassFullName);
            return this;
        }
    }
}
