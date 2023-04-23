using System;
using System.Linq.Expressions;
using FluentCaching.Keys.Builders;
// ReSharper disable UnusedMember.Global

namespace FluentCaching.Configuration.PolicyBuilders.Keys;

public class CachingKeyPolicyBuilder<TEntity>
{
    private static readonly string ClassName = typeof(TEntity).Name;
    private static readonly string ClassFullName = typeof(TEntity).FullName!;

    private readonly IKeyBuilder _keyBuilder;

    internal CachingKeyPolicyBuilder(IKeyBuilder keyBuilder)
    {
        _keyBuilder = keyBuilder;
    }

    public CombinedCachingKeyPolicyBuilder<TEntity> UseAsKey<TValue>(Expression<Func<TEntity, TValue>> valueGetter)
    {
        _keyBuilder.AppendExpression(valueGetter);
        return new CombinedCachingKeyPolicyBuilder<TEntity>(_keyBuilder);
    }

    public CombinedCachingKeyPolicyBuilder<TEntity> UseAsKey<TValue>(TValue value)
    {
        _keyBuilder.AppendStatic<TEntity, TValue>(value);
        return new CombinedCachingKeyPolicyBuilder<TEntity>(_keyBuilder);
    }

    public CombinedCachingKeyPolicyBuilder<TEntity> UseClassNameAsKey()
    {
        _keyBuilder.AppendStatic<TEntity, string>(ClassName);
        return new CombinedCachingKeyPolicyBuilder<TEntity>(_keyBuilder);
    }

    public CombinedCachingKeyPolicyBuilder<TEntity> UseClassFullNameAsKey()
    {
        _keyBuilder.AppendStatic<TEntity, string>(ClassFullName);
        return new CombinedCachingKeyPolicyBuilder<TEntity>(_keyBuilder);
    }
}