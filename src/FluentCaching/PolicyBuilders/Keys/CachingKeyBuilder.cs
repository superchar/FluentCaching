using System;
using System.Linq.Expressions;
using FluentCaching.Keys;
using FluentCaching.Keys.Builders;

namespace FluentCaching.PolicyBuilders.Keys
{
    public class CachingKeyBuilder<T>
        where T : class
    {
        private static readonly string ClassName = typeof(T).Name;
        private static readonly string ClassFullName = typeof(T).FullName;

        private readonly IKeyBuilder<T> _keyBuilder;
        
        internal CachingKeyBuilder() : this(new KeyBuilder<T>())
        {
        }
        
        internal CachingKeyBuilder(IKeyBuilder<T> keyBuilder)
        {
            _keyBuilder = keyBuilder;
        }

        public CombinedCachingKeyBuilder<T> UseAsKey<TValue>(Expression<Func<T, TValue>> valueGetter)
        {
            _keyBuilder.AppendExpression(valueGetter);
            return new CombinedCachingKeyBuilder<T>(_keyBuilder);
        }

        public CombinedCachingKeyBuilder<T> UseAsKey<TValue>(TValue value)
        {
            _keyBuilder.AppendStatic(value);
            return new CombinedCachingKeyBuilder<T>(_keyBuilder);
        }

        public CombinedCachingKeyBuilder<T> UseClassNameAsKey()
        {
            _keyBuilder.AppendStatic(ClassName);
            return new CombinedCachingKeyBuilder<T>(_keyBuilder);
        }

        public CombinedCachingKeyBuilder<T> UseClassFullNameAsKey()
        {
            _keyBuilder.AppendStatic(ClassFullName);
            return new CombinedCachingKeyBuilder<T>(_keyBuilder);
        }
    }
}
