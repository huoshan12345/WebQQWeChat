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
        /// <summary>
        /// 连接两个字节数组
        /// </summary>
        /// <param name="b1"></param>
        /// <param name="b2"></param>
        /// <returns></returns>
        private static byte[] JoinBytes(byte[] b1, byte[] b2)
        {
            var b3 = new byte[b1.Length + b2.Length];
            Array.Copy(b1, b3, b1.Length);
            Array.Copy(b2, 0, b3, b1.Length, b2.Length);
            return b3;
        }

        /// <summary>
        /// 加密字符串，并转换十六进制表示的字符串
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string Md5(string input)
        {
            return Encoding.UTF8.GetBytes(input).ToMd5String();
        }

        /// <summary>
        /// 对QQ信息进行加密
        /// </summary>
        /// <param name="uin">QQ号</param>
        /// <param name="password"></param>
        /// <param name="verifyCode"></param>
        /// <returns></returns>
        private static string Md5QQ(long uin, string password, string verifyCode)
        {
            return Md5QQ(uin, password.ToUtf8Bytes().ToMd5(), verifyCode);
        }

        /// <summary>
        /// 对QQ信息进行加密
        /// </summary>
        /// <param name="uin">QQ号</param>
        /// <param name="md5Pwd"></param>
        /// <param name="verifyCode">验证码</param>
        /// <returns></returns>
        private static string Md5QQ(long uin, byte[] md5Pwd, string verifyCode)
        {
            var b1 = md5Pwd;
            var b2 = BitConverter.GetBytes(uin);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(b2); // 此处要用大端模式
            }
            var s1 = JoinBytes(b1, b2).ToMd5String();
            return Md5(s1 + verifyCode.ToUpper());
        }

        private static string LongToHexString(long uin)
        {
            var buffer = BitConverter.GetBytes(uin);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(buffer); // 此处要用大端模式
            }
            var builder = new StringBuilder();
            for (var i = 0; i < buffer.Length; i++)
            {
                builder.Append(buffer[i].ToString("X2"));
            }
            return builder.ToString();
        }

        /// <summary>
        /// 判断是不是MD5字符串
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private static bool IsMd5(string input)
        {
            return RegMd5.IsMatch(input);
        }

        private static string RsaQQ(long uin, string password, string verifyCode)
        {
            var js = Resource.LoadLocalResource("encrypt.js", stream => stream.ToString(Encoding.UTF8));
            object[] args = { password, uin, verifyCode.ToUpper(), IsMd5(password).ToString().ToLower() };
            var code = string.Format("getEncryption('{0}','{1}','{2}',{3})", args);
            var engine = new Jint.Engine();
            engine.Execute(js);
            var s = engine.Execute(code).GetCompletionValue().AsString();
            return s;
        }

        public static string EncryptQQ(long uin, string password, string verifyCode)
        {
            return RsaQQ(uin, password, verifyCode);
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
    }
}
