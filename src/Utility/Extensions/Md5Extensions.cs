using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Utility.Extensions
{
    public static class Md5Extensions
    {
        public static string ToMd5String(this byte[] input)
        {
            return input.ToMd5().ToHexString();
        }

        public static byte[] ToMd5(this byte[] input)
        {
            return MD5.Create().ComputeHash(input);
        }
    }
}
