using System;
using System.Collections.Generic;
using System.Linq;

namespace Shaman.Common.Extension
{
    public static class EnumerableExt
    {
        public static void DoForEach<T>(this IEnumerable<T> enumerable, Action<T> forEach)
        {
            foreach (T item in enumerable)
            {
                forEach(item);
            }
        }

        public static string AggregateList<T>(this IEnumerable<T> list, string text)
        {
            if (list == null || list.Count() == 0)
            {
                return string.Empty;
            }

            var x = list.Aggregate(string.Empty, (current, item) => current + (item + text));
            return x.Substring(0, x.Length - (text == null ? 0 : text.Length));
        }
    }
}