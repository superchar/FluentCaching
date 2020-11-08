
using System;
using System.Text;

namespace FluentCaching.Api.Key
{
    public class CombinedCachingKeyBuilder<T>
    {
        private readonly T _targetObject;

        private readonly StringBuilder _key;

        public CombinedCachingKeyBuilder(T targetObject, StringBuilder key)
        {
            _targetObject = targetObject;
            _key = key;
        }

        public CachingOptionsBuilder And()
        {
            return new CachingOptionsBuilder(_key.ToString(), _targetObject);
        }

        public CombinedCachingKeyBuilder<T> CombinedWithSelf()
        {
            _key.Append(KeyBuildingHelper.GetStringValue(_targetObject));

            return this;
        }

        public CombinedCachingKeyBuilder<T> CombinedWith<TValue>(Func<T, TValue> valueGetter)
        {
            _key.Append(KeyBuildingHelper.GetStringValue(_targetObject, valueGetter));

            return this;
        }

        public CombinedCachingKeyBuilder<T> CombinedWith<TValue>(TValue value)
        {
            _key.Append(KeyBuildingHelper.GetStringValue(value));

            return this;
        }
    }
}
