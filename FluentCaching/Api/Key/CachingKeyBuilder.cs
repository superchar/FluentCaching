
using System;
using System.Text;

namespace FluentCaching.Api.Key
{
    public class CachingKeyBuilder<T>
    {
        private readonly T _targetObject;

        private readonly StringBuilder _key = new StringBuilder();

        public CachingKeyBuilder(T targetObject)
        {
            _targetObject = targetObject;
        }

        public CombinedCachingKeyBuilder<T> UseSelfAsKey()
        {
            _key.Append(KeyBuildingHelper.GetStringValue(_targetObject));

            return new CombinedCachingKeyBuilder<T>(_targetObject, _key);
        }

        public CombinedCachingKeyBuilder<T> UseAsKey<TValue>(Func<T, TValue> valueGetter)
        {
            _key.Append(KeyBuildingHelper.GetStringValue(_targetObject, valueGetter));

            return new CombinedCachingKeyBuilder<T>(_targetObject, _key);
        }

        public CombinedCachingKeyBuilder<T> UseAsKey<TValue>(TValue value)
        {
            _key.Append(KeyBuildingHelper.GetStringValue(value));

            return new CombinedCachingKeyBuilder<T>(_targetObject, _key);
        }
    }
}
