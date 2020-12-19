
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using FluentCaching.Keys;

namespace FluentCaching.Api.Keys
{
    public class CachingKeyBuilder<T> 
        where T : class
    {
        private readonly PropertyTracker<T> _propertyTracker = new PropertyTracker<T>();

        public CombinedCachingKeyBuilder<T> UseSelfAsKey()
        {
            _propertyTracker.TrackSelf();

            return new CombinedCachingKeyBuilder<T>(_propertyTracker);
        }

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
    }
}
