using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Utility.Extensions;

namespace Utility.Helpers
{
    public static class EnumHelper
    {
        public static IEnumerable<T> GetValues<T>() where T : struct, IConvertible
        {
            return Enum.GetValues(typeof(T)).Cast<T>();
        }

        public static T ParseFromStrNum<T>(string number, T defaultValue) where T : struct, IConvertible
        {
            int val;
            if (int.TryParse(number, out val))
            {
                if (typeof(T).GetTypeInfo().IsEnumDefined(val))
                    return (T)Enum.ToObject(typeof(T), val); ;
            }
            return defaultValue;
        }

        public static T ParseFromStrNum<T>(string number) where T : struct, IConvertible
        {
            int val;
            if (int.TryParse(number, out val))
            {
                if (typeof(T).GetTypeInfo().IsEnumDefined(val))
                    return (T)Enum.ToObject(typeof(T), val); ;
            }
            throw new ArgumentOutOfRangeException(nameof(number));
        }
    }
}
