using System;
using FluentCaching.Keys.Builders.KeyParts.Extensions;
using FluentCaching.Keys.Models;

namespace FluentCaching.Keys.Builders.KeyParts;

internal class StaticKeyPartBuilder : IKeyPartBuilder
{
    private string _keyPart;

    private StaticKeyPartBuilder()
    {
    }
    
    public bool IsDynamic => false;

    public static StaticKeyPartBuilder Create<TValue>(TValue value)
        => new StaticKeyPartBuilder().AppendStatic(value);

    public string Build(KeyContext keyContext) => _keyPart;

    private StaticKeyPartBuilder AppendStatic<TValue>(TValue value)
    {
        _keyPart = value?
            .ToString();
        _keyPart.ThrowIfKeyPartIsNullOrEmpty();

        return this;
    }
}