using System.Collections.Generic;

namespace SalesManager
{
    internal static class ExtensionMethods
    {
        /// <summary>
        /// Indicates whether the specified string is <c>null</c> or an <see cref="System.String.Empty"/> string.
        /// </summary>
        /// <param name="value">The string to test.</param>
        /// <returns>
        /// <c>true</c> if the specified string is <c>null</c> or an <see cref="System.String.Empty"/> string.
        /// </returns>
        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }

        /// <summary>
        /// Returns the value associated with the specified key in a dictionary the or null.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="dict">The dictionary to look up the key.</param>
        /// <param name="key">The key whose value is to be returned.</param>
        /// <returns>
        /// The value associated with <paramref name="key"/> or <c>null</c> if <paramref name="key"/> 
        /// does not exist in the dictionary.
        /// </returns>
        public static TValue ValueOrNull<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key)
        {
            TValue value;
            dict.TryGetValue(key, out value);

            return value;
        }
    }
}