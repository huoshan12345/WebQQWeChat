using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iQQ.Net.WebQQCore.Util
{
    public class UrlUtils
    {
        public static string GetOrigin(string url)
        {
            return url.Substring(0, url.LastIndexOf("/"));
        }
    }
}
