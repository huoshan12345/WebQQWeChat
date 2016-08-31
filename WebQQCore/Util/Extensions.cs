using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace iQQ.Net.WebQQCore.Util
{

    /// <summary>DataList的Add方法的选项</summary>
    public enum AddChoice
    {
        /// <summary>直接添加，有重复时忽略</summary>
        IgnoreDuplication,
        /// <summary>若存在，则更新数值</summary>
        Update,
        /// <summary>直接添加，有重复时异常</summary>
        NotIgnoreDuplication,

    }

    public static class Extensions
    {
        public static string ToString(this Stream stream, Encoding encoding)
        {
            using (var sr = new StreamReader(stream, encoding ?? Encoding.UTF8))
            {
                var text = sr.ReadToEnd();
                return text;
            }
        }

        public static byte[] ToBytes(this Stream stream)
        {
            var bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            // 设置当前流的位置为流的开始
            stream.Seek(0, SeekOrigin.Begin);
            return bytes;
        }

        public static Stream ToStream(this byte[] bytes)
        {
            var stream = new MemoryStream(bytes);
            return stream;
        }

        public static void Add<TK, TV>(this Dictionary<TK, List<TV>> dic, TK key, TV value)
        {
            if (dic.ContainsKey(key))
            {
                dic[key].Add(value);
            }
            else
            {
                dic.Add(key, new List<TV>() { value });
            }
        }


        /// <summary>更新：如果不存在，则添加；存在，则替换。
        /// <para>不更新：如果不存在，则添加；存在，则忽略。</para></summary>
        public static void Add<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue value, AddChoice addChoice)
        {
            switch (addChoice)
            {
                case AddChoice.Update:
                    dict[key] = value;
                    return;

                case AddChoice.IgnoreDuplication:
                    return;

                case AddChoice.NotIgnoreDuplication:
                    throw new ArgumentException("已存在具有相同键的元素");
            }
        }

        public static TValue GetValueOrDefault<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue defaultValue = default(TValue))
        {
            if (dict.ContainsKey(key)) return dict[key];
            else return defaultValue;
        }

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
            return GetAllCookies(cc).Where(item => string.Compare(item.Name,name, StringComparison.OrdinalIgnoreCase) == 0);
        }

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
                builder.Append(buffer[i].ToString("X2"));
            }
            return builder.ToString();
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

        public static bool IsNullOrEmpty(this ICollection col)
        {
            return col == null || col.Count == 0;
        }

        public static bool IsNullOrEmpty<T>(this ICollection<T> col)
        {
            return col == null || col.Count == 0;
        }
    }
}
