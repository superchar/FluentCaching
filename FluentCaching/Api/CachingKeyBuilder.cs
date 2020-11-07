
using System;

namespace FluentCaching.Api
{
    public class CachingKeyBuilder<T>
    {
        private readonly T _targetObject;

        private string _key;

        public CachingKeyBuilder(T targetObject)
        {
            _targetObject = targetObject;
        }

        public CachingOptionsBuilder UseSelfAsKey()
        {
            if (_targetObject == null)
            {
                ThrowKeyNullException();
            }

            // ReSharper disable once PossibleNullReferenceException
            _key = _targetObject.ToString();
            return new CachingOptionsBuilder(_key, _targetObject);
        }

        public CachingOptionsBuilder UseAsKey<TValue>(Func<T, TValue> valueGetter)
        {
            _key = valueGetter(_targetObject)?.ToString();

            if (_key == null)
            {
                ThrowKeyNullException();
            }

            return new CachingOptionsBuilder(_key, _targetObject);
        }

        private static void ThrowKeyNullException() => throw new ArgumentNullException("key", "Caching key cannot be null");
    }
}
