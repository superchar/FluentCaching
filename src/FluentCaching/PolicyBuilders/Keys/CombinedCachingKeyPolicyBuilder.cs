using System;
using System.Linq.Expressions;
using FluentCaching.Keys;
using FluentCaching.Keys.Builders;
using FluentCaching.PolicyBuilders.Ttl;

namespace FluentCaching.PolicyBuilders.Keys
{
    public class CombinedCachingKeyPolicyBuilder<T>
    {
        private static readonly string ClassName = typeof(T).Name;
        private static readonly string ClassFullName = typeof(T).FullName;

        private readonly IKeyBuilder _keyBuilder;

        internal CombinedCachingKeyPolicyBuilder(IKeyBuilder keyBuilder)
        {
            _keyBuilder = keyBuilder;
        }

        public TtlTypePolicyBuilder And() => new (_keyBuilder);

        public CombinedCachingKeyPolicyBuilder<T> CombinedWith<TValue>(Expression<Func<T, TValue>> valueGetter)
        {
            _keyBuilder.AppendExpression(valueGetter);
            return this;
        }

        public CombinedCachingKeyPolicyBuilder<T> CombinedWith<TValue>(TValue value)
        {
            _keyBuilder.AppendStatic(value);
            return this;
        }

        public CombinedCachingKeyPolicyBuilder<T> CombinedWithClassName()
        {
            _keyBuilder.AppendStatic(ClassName);
            return this;
        }

        public CombinedCachingKeyPolicyBuilder<T> CombinedWithClassFullName()
        {
            _keyBuilder.AppendStatic(ClassFullName);
            return this;
        }
    }
}
