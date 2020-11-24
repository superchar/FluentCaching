﻿
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
        public static CachingKeyBuilder<T> Empty { get; } = new CachingKeyBuilder<T>();

        private readonly PropertyTracker _propertyTracker;

        private readonly Key<T> _key;

        public CachingKeyBuilder(T targetObject = null, IDictionary<string, object> valueSource = null)
        {
            _propertyTracker = PropertyTracker.Create(targetObject, valueSource);
            _key = Key<T>.Create(targetObject, valueSource);
        }

        public CombinedCachingKeyBuilder<T> UseSelfAsKey()
        {
            _key.AppendSelf();
            
            _propertyTracker.TrackSelf();

            return new CombinedCachingKeyBuilder<T>(_key, _propertyTracker);
        }

        public CombinedCachingKeyBuilder<T> UseAsKey<TValue>(Expression<Func<T, TValue>> valueGetter)
        {
            _key.AppendProperty(valueGetter);

            _propertyTracker.TrackProperty(valueGetter);
            
            return new CombinedCachingKeyBuilder<T>(_key, _propertyTracker);
        }

        public CombinedCachingKeyBuilder<T> UseAsKey<TValue>(TValue value)
        {
            _key.AppendValue(value);

            return new CombinedCachingKeyBuilder<T>(_key, _propertyTracker);
        }
    }
}