using System.Collections.Generic;
using System.Linq;

namespace SalesManager
{
    internal static class ExtensionMethods
    {
        /// <summary>
        /// Concatenates a specified <see cref="System.String"/> enumerable by placing the specified
        /// separator <see cref="System.String"/> between each element, yielding a single concatenated 
        /// string.
        /// </summary>
        /// <param name="separator">
        /// The separator to place between the elements of <paramref name="value"/>.
        /// </param>
        /// <param name="value">The string enumerable to be concatenated.</param>
        /// <returns>
        /// A <see cref="System.String"/> consisting of the elements of <paramref name="value"/> 
        /// interspersed with the separator string.
        /// </returns>
        public static string Join(this string separator, IEnumerable<string> value)
        {
            return string.Join(separator, value.ToArray());
        }

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