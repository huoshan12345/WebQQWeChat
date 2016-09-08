using System.Text;

namespace HttpActionTools.Extensions
{
    public static class StringBuilderExtensions
    {
        public static StringBuilder AppendLineIf(this StringBuilder sb, string value, bool condition)
        {
            if (condition)
            {
                sb.AppendLine(value);
            }
            return sb;
        }
    }
}
