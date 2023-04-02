using System;

namespace FluentCaching.Extensions;

public static class TypeExtensions
{
    public static string ToFullNameString(this Type type)
        => $"type - '{type.FullName}'";
}