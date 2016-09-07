using System.Security.Cryptography;

namespace HttpActionTools.Extensions
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
