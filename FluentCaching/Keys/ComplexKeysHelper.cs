using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace FluentCaching.Keys
{
    internal class ComplexKeysHelper
    {
        private static readonly ConcurrentDictionary<Type, List<PropertyDescriptor>> PropertiesCache =
            new ConcurrentDictionary<Type, List<PropertyDescriptor>>();

        public static List<PropertyDescriptor> GetProperties(object complexKey) => PropertiesCache
            .GetOrAdd(complexKey.GetType(), GetProperties);


        private static List<PropertyDescriptor> GetProperties(Type type) => TypeDescriptor.GetProperties(type)
            .Cast<PropertyDescriptor>()
            .ToList();

    }
}
