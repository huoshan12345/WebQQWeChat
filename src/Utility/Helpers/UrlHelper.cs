using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Utility.Helper
{
    public class UrlHelper
    {
        public static string GetOrigin(string url)
        {
            return url.Substring(0, url.LastIndexOf("/", StringComparison.Ordinal));
        }
    }
}
