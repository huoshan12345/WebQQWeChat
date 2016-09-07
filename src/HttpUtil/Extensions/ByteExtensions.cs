using System;
using System.Text;

namespace HttpActionTools.Extensions
{
    public static class ByteExtensions
    {
        public static string ToHexString(this byte[] bytes)
        {
            var builder = new StringBuilder();
            foreach (var @byte in bytes)
            {
                builder.Append(@byte.ToString("X2"));
            }
            return builder.ToString();
        }

        public static byte[] ToBytes(this bool num) => BitConverter.GetBytes(num);

        public static byte[] ToBytes(this char num) => BitConverter.GetBytes(num);

        public static byte[] ToBytes(this short num) => BitConverter.GetBytes(num);

        public static byte[] ToBytes(this int num) => BitConverter.GetBytes(num);

        public static byte[] ToBytes(this long num) => BitConverter.GetBytes(num);

        public static byte[] ToBytes(this ushort num) => BitConverter.GetBytes(num);

        public static byte[] ToBytes(this uint num) => BitConverter.GetBytes(num);

        public static byte[] ToBytes(this ulong num) => BitConverter.GetBytes(num);

        public static byte[] ToBytes(this float num) => BitConverter.GetBytes(num);

        public static byte[] ToBytes(this double num) => BitConverter.GetBytes(num);
    }
}
