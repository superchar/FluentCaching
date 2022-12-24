using FluentCaching.Keys.Exceptions;

namespace FluentCaching.Keys.Builders.KeyParts.Extensions;

public static class KeyPartExtensions
{
    public static string ThrowIfKeyPartIsNullOrEmpty(this string part)
    {
        if (string.IsNullOrEmpty(part))
        {
            throw new KeyPartMissingException();
        }

        return part;
    }
}