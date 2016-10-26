using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility.Helper
{
    public static class StringHelper
    {

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
    }
}
