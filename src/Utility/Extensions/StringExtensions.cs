using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Utility.Extensions
{
    public static class StringExtensions
    {
        public static byte[] ToUtf8Bytes(this string input)
        {
            return Encoding.UTF8.GetBytes(input);
        }

        public static byte[] ToBytes(this string input, Encoding encoding)
        {
            return encoding.GetBytes(input);
        }

        public static string UrlEncode(this string url)
        {
            return WebUtility.UrlEncode(url);
        }

        public static string UrlDecode(this string url)
        {
            return WebUtility.UrlDecode(url);
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

        public static string JoinWith(this IEnumerable<string> strs, string separator)
        {
            return string.Join(separator, strs);
        }
    }
}
