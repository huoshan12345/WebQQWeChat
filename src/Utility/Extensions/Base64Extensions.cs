using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Utility.Extensions
{
    public static class Base64Extensions
    {
        public static string ToBase64(this byte[] bytes)
        {
            return Convert.ToBase64String(bytes);
        }

        public static byte[] FromBase64(this string str)
        {
            return Convert.FromBase64String(str);
        }
    }
}
