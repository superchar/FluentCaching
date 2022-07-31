using System;
using System.Linq.Expressions;
using FluentCaching.Keys.Builders;

namespace FluentCaching.PolicyBuilders.Keys
{
    public class CachingKeyPolicyBuilder<T>
        where T : class
    {
        private static readonly string ClassName = typeof(T).Name;
        private static readonly string ClassFullName = typeof(T).FullName;

        private readonly IKeyBuilder<T> _keyBuilder;
        
        internal CachingKeyPolicyBuilder() : this(new KeyBuilder<T>())
        {
        }
        
        internal CachingKeyPolicyBuilder(IKeyBuilder<T> keyBuilder)
        {
            _keyBuilder = keyBuilder;
        }

        public CombinedCachingKeyPolicyBuilder<T> UseAsKey<TValue>(Expression<Func<T, TValue>> valueGetter)
        {
            _keyBuilder.AppendExpression(valueGetter);
            return new CombinedCachingKeyPolicyBuilder<T>(_keyBuilder);
        }

        public CombinedCachingKeyPolicyBuilder<T> UseAsKey<TValue>(TValue value)
        {
            _keyBuilder.AppendStatic(value);
            return new CombinedCachingKeyPolicyBuilder<T>(_keyBuilder);
        }

        public CombinedCachingKeyPolicyBuilder<T> UseClassNameAsKey()
        {
            _keyBuilder.AppendStatic(ClassName);
            return new CombinedCachingKeyPolicyBuilder<T>(_keyBuilder);
        }

        public CombinedCachingKeyPolicyBuilder<T> UseClassFullNameAsKey()
        {
            _keyBuilder.AppendStatic(ClassFullName);
            return new CombinedCachingKeyPolicyBuilder<T>(_keyBuilder);
        }
    }
}
