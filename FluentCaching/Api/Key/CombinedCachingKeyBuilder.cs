
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace FluentCaching.Api.Key
{
    public class CombinedCachingKeyBuilder<T>
        where T : class
    {
        private readonly Key<T> _key;

        private readonly PropertyTracker _propertyTracker;

        internal CombinedCachingKeyBuilder(Key<T> key, PropertyTracker propertyTracker)
        {
            _key = key;
            _propertyTracker = propertyTracker;
        }

        public CachingOptionsBuilder And()
        {
            return new CachingOptionsBuilder(_key.ToString());
        }

        public CombinedCachingKeyBuilder<T> CombinedWithSelf()
        {
            _key.AppendSelf();

            _propertyTracker.TrackSelf();
    
            return this;
        }

        public CombinedCachingKeyBuilder<T> CombinedWith<TValue>(Expression<Func<T, TValue>> valueGetter)
        {
            _key.AppendProperty(valueGetter);

            _propertyTracker.TrackProperty(valueGetter);

            return this;
        }

        public CombinedCachingKeyBuilder<T> CombinedWith<TValue>(TValue value)
        {
            _key.AppendValue(value);

            return this;
        }
    }
}
