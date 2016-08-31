using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace iQQ.Net.WebQQCore.Util.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// 将字符串MD5加密为字节数组
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static byte[] Md5ToArray(this string input)
        {
            return MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(input));
        }

        /// <summary>
        /// 将字符串MD5加密为字符串
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string Md5ToString(this string input)
        {
            var buffer = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(input));
            var builder = new StringBuilder();
            for (var i = 0; i < buffer.Length; i++)
            {
                builder.Append(i.ToString("X2"));
            }
            return builder.ToString();
        }


        public static string UrlEncode(this string url)
        {
            return HttpUtility.UrlEncode(url, Encoding.UTF8);
        }

        public static string UrlEncode(this string url, string encoding)
        {
            return HttpUtility.UrlEncode(url, Encoding.GetEncoding(encoding));
        }

        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        public static bool IsNullOrWhiteSpace(this string str)
        {
            return string.IsNullOrWhiteSpace(str);
        }

        public static string RegexReplace(this string str, string rex, string replacement)
        {
            return Regex.Replace(str, rex, replacement);
        }

        public static string GetOrEmpty(this string str)
        {
            return str ?? "";
        }
    }
}
