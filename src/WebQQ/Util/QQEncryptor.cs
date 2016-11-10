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
        private static readonly string[] HexChars = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "A", "B", "C", "D", "E", "F" };

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
