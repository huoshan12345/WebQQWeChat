using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace iQQ.Net.WebQQCore.Util.Extensions
{
    public static class HttpExtensions
    {
        public static IEnumerable<Cookie> GetAllCookies(this CookieContainer cc)
        {
            var table = (Hashtable)cc.GetType().InvokeMember("m_domainTable",
            BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.Instance,
            null, cc, new object[] { });

            foreach (var pathList in table.Values)
            {
                var lstCookieCol = (SortedList)pathList.GetType().InvokeMember("m_list",
                BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.Instance,
                null, pathList, new object[] { });
                foreach (CookieCollection colCookies in lstCookieCol.Values)
                {
                    foreach (var c in colCookies.OfType<Cookie>())
                    {
                        yield return c;
                    }
                }
            }
        }

        public static IEnumerable<Cookie> GetCookies(this CookieContainer cc, string name)
        {
            return GetAllCookies(cc).Where(item => string.Compare(item.Name, name, StringComparison.OrdinalIgnoreCase) == 0);
        }
    }
}
