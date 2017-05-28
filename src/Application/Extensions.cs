using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ImageSharp;
using WebQQ.Util;

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

        public static void Clear<T>(this BlockingCollection<T> blockingCollection)
        {
            if (blockingCollection == null)
            {
                throw new ArgumentNullException(nameof(blockingCollection));
            }
            while (blockingCollection.Count > 0)
            {
                blockingCollection.TryTake(out var _);
            }
        }
    }
}
