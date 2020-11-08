
using System;

namespace FluentCaching.Api.Key
{
    internal static class KeyBuildingHelper
    {
        public static string GetStringValue<T, TValue>(T targetObject, Func<T, TValue> valueGetter)
        {
            var key = valueGetter(targetObject)?.ToString();

            if (key == null)
            {
                ThrowKeyNullException();
            }

            return key;
        }

        public static string GetStringValue<T>(T targetObject)
        {
            if (targetObject == null)
            {
                ThrowKeyNullException();
            }

            // ReSharper disable once PossibleNullReferenceException
            return targetObject.ToString();
        }

        private static void ThrowKeyNullException() => throw new ArgumentNullException("key", "Caching key cannot be null");

    }
}
