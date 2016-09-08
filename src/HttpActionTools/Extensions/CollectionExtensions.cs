using System.Collections;
using System.Collections.Generic;

namespace HttpActionTools.Extensions
{
    public static class CollectionExtensions
    {
        public static bool IsNullOrEmpty(this ICollection col)
        {
            return col == null || col.Count == 0;
        }

        public static void AddWhenNotNull<T>(this ICollection<T> col, T item)
        {
            if(item != null) col.Add(item);
        }

        public static void AddRangeSafely<T>(this ICollection<T> col, IEnumerable<T> items)
        {
            if(items == null) return;

            var list = col as List<T>;
            if (list != null)
            {
                list.AddRange(items);
            }
            else
            {
                foreach (var item in items)
                {
                    col.Add(item);
                }
            }
        }
    }
}
