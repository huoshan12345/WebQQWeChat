using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WebQQ.Util
{
    public static class Extesions
    {
        public static string ToJson(this object obj, Formatting formatting = Formatting.None)
        {
            return JsonConvert.SerializeObject(obj, formatting);
        }

        public static JObject ToJsonObj(this string str)
        {
            return JObject.Parse(str);
        }

        public static JObject ToJsonObj(this JToken token)
        {
            return token.ToObject<JObject>();
        }

        public static string ToSimpleString(this JToken obj)
        {
            return obj.ToString(Formatting.None);
        }

        public static int ToInt(this JToken token)
        {
            return token.ToObject<int>();
        }

        public static T ToEnum<T>(this string value, T defaultValue = default(T)) where T : struct, IConvertible
        {
            if (string.IsNullOrEmpty(value))
            {
                return defaultValue;
            }

            T result;
            return Enum.TryParse(value, true, out result) ? result : defaultValue;
        }
    }
}
