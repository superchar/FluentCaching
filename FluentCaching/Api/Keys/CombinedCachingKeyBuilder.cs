using System;
using System.Linq.Expressions;
using FluentCaching.Keys;

namespace FluentCaching.Api.Keys
{
    public class CombinedCachingKeyBuilder<T>
        where T : class
    {
        private readonly PropertyTracker<T> _propertyTracker;

        internal CombinedCachingKeyBuilder(PropertyTracker<T> propertyTracker)
        {
            _propertyTracker = propertyTracker;
        }

        public CachingOptionsBuilder And() => new CachingOptionsBuilder(_propertyTracker);

        public CombinedCachingKeyBuilder<T> CombinedWithSelf()
        {
            _propertyTracker.TrackSelf();
            return this;
        }

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
    }
}
