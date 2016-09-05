using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace iQQ.Net.WebQQCore.Util.Extensions
{
    public static class Md5Extensions
    {
        public static string ToMd5String(this byte[] input)
        {
            return input.ToMd5Bytes().ToHexString();
        }

        public static byte[] ToMd5Bytes(this byte[] input)
        {
            return MD5.Create().ComputeHash(input);
        }
    }
}
