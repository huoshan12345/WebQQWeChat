using System;

namespace WebQQ.Util
{
    public class UrlUtils
    {
        public static string GetOrigin(string url)
        {
            return url.Substring(0, url.LastIndexOf("/", StringComparison.Ordinal));
        }
    }
}
