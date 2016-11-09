using FxUtility.Extensions;
using System;
using System.Text;
using System.Text.RegularExpressions;

namespace WebQQ.Util
{
    /// <summary>
    /// QQ加密解码
    /// </summary>
    public static class QQEncryptor
    {
        private static readonly Regex RegMd5 = new Regex(@"^([a-fA-F0-9]{32})$");

        private static readonly string[] HexChars = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "A", "B", "C", "D", "E", "F" };

        /// <summary>
        /// 判断是不是MD5字符串
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private static bool IsMd5(string input)
        {
            return RegMd5.IsMatch(input);
        }

        /// <summary>
        /// 登录邮箱时用到的，auth_token
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static long Time33(string str)
        {
            long hash = 0;
            for (int i = 0, length = str.Length; i < length; i++)
            {
                hash = hash * 33 + str[i];
            }
            return hash % 4294967296L;
        }

        /// <summary>
        /// 获取好友列表时计算的Hash参数 v2014.06.14更新
        /// </summary>
        /// <param name="uin"></param>
        /// <param name="ptwebqq"></param>
        /// <returns></returns>
        public static string GetHash(string uin, string ptwebqq)
        {
            const string url = "https://raw.githubusercontent.com/im-qq/webqq-core/master/src/main/resources/hash.js";
            var js = Resource.LoadResourceAsync("hash.js", url, item => item.ToString(Encoding.UTF8)).Result;
            object[] args = { uin, ptwebqq };
            var code = string.Format("hash('{0}','{1}')", args);
            var engine = new Jint.Engine();
            engine.Execute(js);
            var s = engine.Execute(code).GetCompletionValue().AsString();
            return s;
        }

        /// <summary>
        /// 计算GTK(gtk啥东东？)这个数在操作群和空间时会用到。
        /// </summary>
        /// <param name="skey"></param>
        /// <returns></returns>
        public static string Gtk(string skey)
        {
            var hash = 5381;
            for (var i = 0; i < skey.Length; i++)
            {
                hash += (hash << 5) + skey[i];
            }
            return (hash & 0x7fffffff).ToString();
        }

        /// <summary>
        /// 用于获取好友列表
        /// 由js改写而来
        /// </summary>
        /// <param name="uin"></param>
        /// <param name="ptwebqq"></param>
        /// <returns></returns>
        public static string Hash(long uin, string ptwebqq)
        {
            var n = new char[4];
            for (var i = 0; i < ptwebqq.Length; i++)
            {
                n[i % 4] ^= ptwebqq[i];
            }
            var u = new[] { "EC", "OK" };
            var v = new char[4];
            v[0] = (char)((uin >> 24 & 255) ^ u[0][0]);
            v[1] = (char)(uin >> 16 & 255 ^ u[0][1]);
            v[2] = (char)(uin >> 8 & 255 ^ u[1][0]);
            v[3] = (char)(uin & 255 ^ u[1][1]);
            var u1 = new char[8];
            for (var i = 0; i < u1.Length; i++)
            {
                u1[i] = i % 2 == 0 ? n[i >> 1] : v[i >> 1];
            }
            var v1 = new StringBuilder(2 * u1.Length);
            foreach (var ch in u1)
            {
                v1.Append(HexChars[ch >> 4 & 15]);
                v1.Append(HexChars[ch & 15]);
            }
            return v1.ToString();
        }
    }
}
