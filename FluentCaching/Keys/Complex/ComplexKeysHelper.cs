using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;

namespace FluentCaching.Keys.Complex
{
    internal class ComplexKeysHelper
    {
        private static readonly ConcurrentDictionary<Type, PropertyAccessor[]> Cache
            = new ConcurrentDictionary<Type, PropertyAccessor[]>();

        private static readonly MethodInfo CallInnerDelegateMethod =
            typeof(ComplexKeysHelper).GetMethod(nameof(CallInnerDelegate),
                BindingFlags.NonPublic | BindingFlags.Static);

        public static PropertyAccessor[] GetProperties(Type type)
            => Cache
                .GetOrAdd(type, _ => _.GetProperties().Select(CreatePropertyAccessor).ToArray());

        private static PropertyAccessor CreatePropertyAccessor(PropertyInfo property)
        {
            var getMethod = property.GetMethod;

            var declaringClass = property.DeclaringType;

            var typeOfResult = property.PropertyType;

            var getMethodDelegateType = typeof(Func<,>).MakeGenericType(declaringClass, typeOfResult);

            var getMethodDelegate = getMethod.CreateDelegate(getMethodDelegateType);

            var callInnerGenericMethodWithTypes = CallInnerDelegateMethod
                .MakeGenericMethod(declaringClass, typeOfResult);

            var result = (Func<object, object>) callInnerGenericMethodWithTypes.Invoke(null, new[] {getMethodDelegate});

            return new PropertyAccessor(property.Name, result);
        }

        private static Func<object, object> CallInnerDelegate<TClass, TResult>(
            Func<TClass, TResult> targetDelegate)
            => instance => targetDelegate((TClass) instance);
    }
}
