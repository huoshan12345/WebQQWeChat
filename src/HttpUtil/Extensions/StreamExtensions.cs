using System.IO;
using System.Text;

namespace HttpActionTools.Extensions
{
    public static class StreamExtensions
    {
        public static string ToString(this Stream stream, Encoding encoding = null)
        {
            using (var sr = new StreamReader(stream, encoding ?? Encoding.UTF8))
            {
                return sr.ReadToEnd();
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
            return new MemoryStream(bytes);
        }
    }
}
