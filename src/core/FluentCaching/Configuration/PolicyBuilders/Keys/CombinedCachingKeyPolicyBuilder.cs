using System;
using System.Linq.Expressions;
using FluentCaching.Configuration.PolicyBuilders.Ttl;
using FluentCaching.Keys.Builders;
// ReSharper disable UnusedMember.Global

namespace FluentCaching.Configuration.PolicyBuilders.Keys;

public class CombinedCachingKeyPolicyBuilder<TEntity>
{
    private static readonly string ClassName = typeof(TEntity).Name;
    private static readonly string ClassFullName = typeof(TEntity).FullName;

    private readonly IKeyBuilder _keyBuilder;

    internal CombinedCachingKeyPolicyBuilder(IKeyBuilder keyBuilder)
    {
        _keyBuilder = keyBuilder;
    }

    public TtlTypePolicyBuilder And() => new (_keyBuilder);

    public CombinedCachingKeyPolicyBuilder<TEntity> CombinedWith<TValue>(Expression<Func<TEntity, TValue>> valueGetter)
    {
        _keyBuilder.AppendExpression(valueGetter);
        return this;
    }

    public CombinedCachingKeyPolicyBuilder<TEntity> CombinedWith<TValue>(TValue value)
    {
        _keyBuilder.AppendStatic<TEntity, TValue>(value);
        return this;
    }

    public CombinedCachingKeyPolicyBuilder<TEntity> CombinedWithClassName()
    {
        _keyBuilder.AppendStatic<TEntity, string>(ClassName);
        return this;
    }

    public CombinedCachingKeyPolicyBuilder<TEntity> CombinedWithClassFullName()
    {
        _keyBuilder.AppendStatic<TEntity, string>(ClassFullName);
        return this;
    }
}