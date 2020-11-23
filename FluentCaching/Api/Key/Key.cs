
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace FluentCaching.Api.Key
{
    internal class Key<T>
    {
        private const string Self = nameof(Self);

        private readonly T _targetObject;

        private readonly Dictionary<string, object> _valueSource;

        private readonly StringBuilder _key = new StringBuilder();

        private Key(T targetObject, Dictionary<string, object> valueSource)
        {
            _targetObject = targetObject;
            _valueSource = valueSource;
        }

        public static Key<T> Create(T targetObject, Dictionary<string, object> valueSource)
        {
            if (targetObject != null || valueSource != null)
            {
                return new Key<T>(targetObject, valueSource);
            }

            return EmptyKey.Instance;
        }

        public virtual void AppendSelf()
        {
            var value = _targetObject != null ? _targetObject : _valueSource[Self];
            _key.Append(GetStringValue(value));
        }

        public virtual void AppendValue<TValue>(TValue value)
        {
            _key.Append(GetStringValue(value));
        }

        public virtual void AppendProperty<TValue>(Expression<Func<T, TValue>> valueGetter)
        {
            string value = null;

            if (_targetObject != null)
            {
                var compiled = valueGetter.Compile();
                value = compiled(_targetObject)?.ToString();
            }
            else
            {
                var valueSourceKey = ((MemberExpression) valueGetter.Body).Member.Name;
                value = _valueSource[valueSourceKey].ToString();
            }

            if (value == null)
            {
                ThrowKeyNullException();
            }

            _key.Append(value);
        }

        public override string ToString() => _key.ToString();

        private static string GetStringValue<TValue>(TValue targetObject)
        {
            if (targetObject == null)
            {
                ThrowKeyNullException();
            }

            // ReSharper disable once PossibleNullReferenceException
            return targetObject.ToString();
        }

        private static void ThrowKeyNullException() => throw new ArgumentNullException("key", "Caching key cannot be null");

        private class EmptyKey : Key<T>
        {
            public static readonly EmptyKey Instance = new EmptyKey(default(T),null);

            private EmptyKey(T targetObject, Dictionary<string, object> valueSource) : base(targetObject, valueSource)
            {
            }

            public override void AppendProperty<TValue>(Expression<Func<T, TValue>> valueGetter)
            {
            }

            public override void AppendSelf()
            {
            }

            public override void AppendValue<TValue>(TValue value)
            {
            }

            public override string ToString() => string.Empty;
        }

    }
}
