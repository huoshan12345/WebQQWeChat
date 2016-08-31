using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iQQ.Net.WebQQCore.Util.Extensions
{
    public static class CollectionExtensions
    {

        public static bool IsNullOrEmpty(this ICollection col)
        {
            return col == null || col.Count == 0;
        }

        public static bool IsNullOrEmpty<T>(this ICollection<T> col)
        {
            return col == null || col.Count == 0;
        }
    }
}
