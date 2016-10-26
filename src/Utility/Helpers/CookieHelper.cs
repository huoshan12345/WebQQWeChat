using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Utility.Helpers
{
    public static class CookieHelper
    {
        public static List<Cookie> GetCookies(string cookieStr)
        {
            return cookieStr.Split(';').Select(m => m.Split('=')).Where(m => m.Length == 2)
                .Select(m => new Cookie(m[0].Trim(), m[1].Trim())).ToList();
        }
    }
}
