using System;
using FluentCaching.Keys.Models;

namespace FluentCaching.Keys.Helpers
{
    internal interface IComplexKeysHelper
    {
        PropertyAccessor[] GetProperties(Type type);
    }
}