using System;
using System.Linq.Expressions;
using FluentCaching.Keys.Builders;

namespace FluentCaching.Configuration.PolicyBuilders.Keys
{
    public class CachingKeyPolicyBuilder<T>
    {
        private static readonly string ClassName = typeof(T).Name;
        private static readonly string ClassFullName = typeof(T).FullName;

        private readonly IKeyBuilder _keyBuilder;
        
        internal CachingKeyPolicyBuilder() : this(new KeyBuilder())
        {
        }
        
        internal CachingKeyPolicyBuilder(IKeyBuilder keyBuilder)
        {
            _keyBuilder = keyBuilder;
        }

        public CombinedCachingKeyPolicyBuilder<T> UseAsKey<TValue>(Expression<Func<T, TValue>> valueGetter)
        {
            _keyBuilder.AppendExpression(valueGetter);
            return new CombinedCachingKeyPolicyBuilder<T>(_keyBuilder);
        }
        
        public CombinedCachingKeyPolicyBuilder<TExternal> UseAsKey<TExternal, TValue>
            (Expression<Func<TExternal, TValue>> valueGetter)
        {
            _keyBuilder.AppendExpression(valueGetter);
            return new CombinedCachingKeyPolicyBuilder<TExternal>(_keyBuilder);
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
