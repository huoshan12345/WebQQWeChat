using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace HttpActionTools.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// 将字符串MD5加密为字节数组
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static byte[] Md5ToBytes(this string input)
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

        public static byte[] ToUTF8Bytes(this string input)
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
