using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility.Extensions
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
