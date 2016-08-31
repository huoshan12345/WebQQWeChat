using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iQQ.Net.WebQQCore.Util.Extensions
{
    public static class ObjectExtensions
    {
        public static T GetOrDefault<T>(this T obj, T defaultValue)
        {
            return obj == null ? defaultValue : obj;
        }

        public static bool IsNull(this object obj)
        {
            return obj == null;
        }
    }
}

