using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace iQQ.Net.WebQQCore.Util
{
    /**
     * 字符串工具类
     *
     * @author solosky
     */
    public static class StringHelper
    {
        public static Dictionary<string, string> QueryStringToDict(string queryString)
        {
            var dict = new Dictionary<string, string>();
            var queryItems = queryString.Split('&');
            foreach (var query in queryItems)
            {
                var pair = query.Split('=');
                if (pair.Length == 2 && !dict.Keys.Contains(pair[0]))
                {
                    dict.Add(pair[0], pair[1]);
                }
            }
            return dict;
        }

        /// <summary>
        /// 把十六进制字符串转在byte[]
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        public static byte[] HexStringToBytes(string hex)
        {
            if (hex.Length == 0)
            {
                return new byte[] { 0 };
            }
            if (hex.Length % 2 == 1)
            {
                hex = "0" + hex;
            }
            var result = new byte[hex.Length / 2];

            for (var i = 0; i < hex.Length / 2; i++)
            {
                result[i] = byte.Parse(hex.Substring(2 * i, 2), System.Globalization.NumberStyles.AllowHexSpecifier);
            }
            return result;
        }

        /**
         * 转义HTML中特殊的字符
         *
         * @param html HTML 内容
         * @return 结果字符串
         */
        public static string QouteHtmlSpecialChars(string html)
        {
            if (html == null) return null;
            string[] specialChars = { "&", "\"", "'", "<", ">" };
            string[] qouteChars = { "&amp;", "&quot;", "&apos;", "&lt;", "&gt;" };
            for (var i = 0; i < specialChars.Length; i++)
            {
                html = html.Replace(specialChars[i], qouteChars[i]);
            }
            return html;
        }

        /**
         * 反转义HTML中特殊的字符
         *
         * @param html HTML 内容
         * @return 结果字符串
         */
        public static string UnqouteHtmlSpecialChars(string html)
        {
            if (html == null) return null;
            string[] specialChars = { "&", "\"", "'", "<", ">", " " };
            string[] qouteChars = { "&amp;", "&quot;", "&apos;", "&lt;", "&gt;", "&nbsp;" };
            for (var i = 0; i < specialChars.Count(); i++)
            {
                html = html.Replace(qouteChars[i], specialChars[i]);
            }
            return html;
        }


        /**
         * 去掉HTML标签
         *
         * @param html HTML 内容
         * @return 去除HTML标签后的结果
         */
        public static string StripHtmlSpecialChars(string html)
        {
            if (html == null) return null;
            html = Regex.Replace(html, "</?[^>]+>", "");
            html = html.Replace("&nbsp;", " ");

            return html;
        }

    }

}
