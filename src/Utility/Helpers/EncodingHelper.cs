using System.IO;
using System.Text;

namespace Utility.Helpers
{
    /// <summary>
    /// 获取文件的编码格式
    /// </summary>
    public static class EncodingHelper
    {
        private static readonly Encoding Utf8WithoutBom = new UTF8Encoding(false);
        /// <summary>
        /// 给定文件的路径，读取文件的二进制数据，判断文件的编码类型
        /// </summary>
        /// <param name="fileName">文件路径</param>
        /// <returns>文件的编码类型</returns>
        public static Encoding GetEncodingType(string fileName)
        {
            using (var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                return GetEncodingType(fs);
            }
        }

        /// <summary>
        /// 通过给定的文件流，判断文件的编码类型
        /// </summary>
        /// <param name="fs">文件流</param>
        /// <returns>文件的编码类型</returns>
        private static Encoding GetEncodingType(FileStream fs)
        {
            var bom = new byte[3];
            var length = fs.Read(bom, 0, 3);
            if (length > 2)
            {
                if (bom[0] == 0x2b && bom[1] == 0x2f && bom[2] == 0x76) return Encoding.UTF7;
                if (bom[0] == 0xEF && bom[1] == 0xBB && bom[2] == 0xBF) return Encoding.UTF8;
                if (bom[0] == 0xFE && bom[1] == 0xFF && bom[2] == 0x00) return Encoding.BigEndianUnicode;// 也就是大端的UTF-16
                if (bom[0] == 0xFF && bom[1] == 0xFE && bom[2] == 0x41) return Encoding.Unicode;// 也就是小端的UTF-16
            }
            fs.Seek(0, SeekOrigin.Begin);
            if (IsUtf8(fs)) return Utf8WithoutBom;

            return Encoding.ASCII;
        }

        // 0XXXXXXX
        // 110XXXXX, 10XXXXXX  
        // 1110XXXX, 10XXXXXX, 10XXXXXX  
        // 11110XXX, 10XXXXXX, 10XXXXXX, 10XXXXXX  
        private static bool IsUtf8(FileStream fs)
        {
            using (var r = new BinaryReader(fs))
            {
                var utf8Flag = 0;
                var asciiFlag = 0;
                for (; fs.Position < fs.Length;)
                {
                    var curByte = r.ReadByte();
                    if ((curByte & 0x80) == 0) asciiFlag++; // 0XXXXXXX
                    else if ((curByte & 0xE0) == 0xC0 && fs.Position < fs.Length - 1) // 110xxxxx 10xxxxxx  
                    {
                        var buff = r.ReadByte();
                        if ((buff & 0x80) != 0x80) return false;
                        utf8Flag++;
                    }
                    else if ((curByte & 0xF0) == 0xE0 && fs.Position < fs.Length - 2) // 1110xxxx 10xxxxxx 10xxxxxx  
                    {
                        var buff = r.ReadBytes(2);
                        if ((buff[0] & 0x80) != 0x80 || (buff[1] & 0x80) != 0x80) return false;
                        utf8Flag++;
                    }
                    else if ((curByte & 0xF8) == 0xF0 && fs.Position < fs.Length - 3) // 11110xxx 10xxxxxx 10xxxxxx 10xxxxxx  
                    {
                        var buff = r.ReadBytes(3);
                        if ((buff[0] & 0x80) != 0x80 || (buff[1] & 0x80) != 0x80 || (buff[2] & 0x80) != 0x80) return false;
                        utf8Flag++;
                    }
                    else return false;
                }
                if (asciiFlag == fs.Length) return true;
                return utf8Flag > 0;
            }
        }
    }
}
