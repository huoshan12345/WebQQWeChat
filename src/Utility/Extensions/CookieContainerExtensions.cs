using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Utility.Extensions
{
    public static class CookieContainerExtensions
    {
        public static void AddRange(this CookieContainer cc, Uri uri, IEnumerable<Cookie> cookies)
        {
            foreach (var cookie in cookies)
            {
                cc.Add(uri, cookie);
            }
        }
    }
}
