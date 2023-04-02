using System;
using FluentCaching.Keys.Builders.KeyParts.Extensions;
using FluentCaching.Keys.Models;

namespace FluentCaching.Keys.Builders.KeyParts;

internal class StaticKeyPartBuilder<TEntity> : IKeyPartBuilder
{
    private static readonly Type EntityType = typeof(TEntity);
    private string _keyPart;
    
    public bool IsDynamic => false;

    public static StaticKeyPartBuilder<TEntity> Create<TValue>(TValue value)
        => new StaticKeyPartBuilder<TEntity>()
            .AppendStatic(value);

    public string Build(KeyContext keyContext) => _keyPart.ThrowIfKeyPartIsNull(EntityType);

    private StaticKeyPartBuilder<TEntity> AppendStatic<TValue>(TValue value)
    {
        _keyPart = value?.ToString();

        return this;
    }
}