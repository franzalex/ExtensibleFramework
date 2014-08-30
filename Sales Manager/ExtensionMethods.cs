using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SalesManager
{
    internal static class ExtensionMethods
    {
        public static TValue ValueOrNull<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key)
        {
            TValue value;
            dict.TryGetValue(key, out value);

            return value;
        }
    }
}
