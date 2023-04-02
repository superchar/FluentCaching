using System;
using FluentCaching.Keys.Exceptions;

namespace FluentCaching.Keys.Builders.KeyParts.Extensions;

public static class KeyPartExtensions
{
    public static string ThrowIfKeyPartIsNull(this string part, Type type) =>
        part ?? throw new KeyPartNullException(type);
}