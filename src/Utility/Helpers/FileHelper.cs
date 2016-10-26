using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Utility.Helpers
{
    public static class FileHelper
    {
        public static string ReadFile(string filePath, Encoding encoding)
        {
            return File.ReadAllText(filePath, encoding);
        }

        public static string ReadFileAutoDetectEncoding(string filePath)
        {
            return ReadFile(filePath, EncodingHelper.GetEncodingType(filePath));
        }

        private static int WriteLines(string filePath, IEnumerable<string> lines, Encoding encoding, FileMode fileMode)
        {
            using (var fs = new FileStream(filePath, fileMode, FileAccess.Write))
            {
                using (var sr = new StreamWriter(fs, encoding ?? Encoding.UTF8))
                {
                    var i = 0;
                    foreach (var line in lines)
                    {
                        if (string.IsNullOrEmpty(line)) continue;
                        sr.WriteLine(line);
                        ++i;
                    }
                    return i;

                }
            }
        }

        public static int WriteLinesAppend(string filePath, IEnumerable<string> lines, Encoding encoding)
        {
            return WriteLines(filePath, lines, encoding, FileMode.OpenOrCreate);
        }

        public static int WriteLines(string filePath, IEnumerable<string> lines, Encoding encoding)
        {
            return WriteLines(filePath, lines, encoding, FileMode.Create);
        }

        public static IEnumerable<string> ReadLines(string filePath, Encoding encoding)
        {
            return File.ReadLines(filePath, encoding);
        }

        public static string[] ReadAllLines(string filePath, Encoding encoding)
        {
            return File.ReadAllLines(filePath, encoding);
        }


    }
}
