using System;

namespace Utility.Extensions
{
    public static class EnumExtensions
    {

        public static int ToInt(this Enum @enum)
        {
            return Convert.ToInt32(@enum);
        }
    }
}
