using System;
using FluentCaching.Keys.Builders.KeyParts.Extensions;
using FluentCaching.Keys.Models;

namespace FluentCaching.Keys.Builders.KeyParts;

internal class StaticKeyPartBuilder<T> : IKeyPartBuilder<T>
    where T : class
{
    private string _keyPart;

    private StaticKeyPartBuilder()
    {
    }
    
    public bool IsDynamic => false;

    public static StaticKeyPartBuilder<T> Create<TValue>(TValue value)
        => new StaticKeyPartBuilder<T>().AppendStatic(value);

    public string Build(KeyContext<T> keyContext) => _keyPart;

    private StaticKeyPartBuilder<T> AppendStatic<TValue>(TValue value)
    {
        _keyPart = value?
            .ToString();
        _keyPart.ThrowIfKeyPartIsNullOrEmpty();

        return this;
    }
}