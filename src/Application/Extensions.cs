using System;
using System.Collections.Generic;
using System.Text;

namespace Application
{
    public static class Extensions
    {
        public static void Add<TKey, TValue, TCol>(this IDictionary<TKey, TCol> dic, TKey key, TValue value)
            where TCol : ICollection<TValue>, new()
        {
            TCol col;
            if (!dic.TryGetValue(key, out col)) col = new TCol();
            col.Add(value);
        }
    }
}
