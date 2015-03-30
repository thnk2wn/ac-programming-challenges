using System;
using System.Collections.Generic;
using System.Linq;

namespace BestComments
{
    public static class EnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var item in source)
                action(item);
        }

        public static double? Median<TColl, TValue>(this IEnumerable<TColl> source, Func<TColl, TValue> selector)
        {
            return source.Select(selector).Median();
        }

        public static double? Median<T>(this IEnumerable<T> source)
        {
            if (Nullable.GetUnderlyingType(typeof(T)) != null)
                source = source.Where(x => x != null);

            var list = source.OrderBy(x=> x).ToList();
            if (!list.Any()) return null;
            var count = list.Count;
            var midpoint = count / 2;
            if (count % 2 == 0)
                return (Convert.ToDouble(list.ElementAt(midpoint - 1)) + Convert.ToDouble(list.ElementAt(midpoint))) / 2.0;
            return Convert.ToDouble(list.ElementAt(midpoint));
        }
    }
}