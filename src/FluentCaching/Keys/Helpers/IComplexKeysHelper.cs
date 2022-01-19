using System;

namespace FluentCaching.Keys.Helpers
{
    internal interface IComplexKeysHelper
    {
        PropertyAccessor[] GetProperties(Type type);
    }
}