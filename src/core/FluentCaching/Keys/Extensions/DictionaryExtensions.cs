using System.Collections.Generic;

namespace FluentCaching.Keys.Extensions
{
    internal static class DictionaryExtensions
    {
        /// <summary>
        /// Returns single key from dictionary without allocation
        /// </summary>
        /// <param name="dictionary"></param>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <returns></returns>
        public static TKey FirstKey<TKey, TValue>(this Dictionary<TKey, TValue> dictionary)
        {
            using var enumerator = dictionary.GetEnumerator();
            enumerator.MoveNext();
            return enumerator.Current.Key;
        }
    }
}