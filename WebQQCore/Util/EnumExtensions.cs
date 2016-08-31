using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace iQQ.Net.WebQQCore.Util
{
    public static class EnumExtension
    {
        private static readonly ConcurrentDictionary<Enum, string> EnumDic = new ConcurrentDictionary<Enum, string>();

        /// <summary>
        /// 获取枚举的描述信息(Descripion)。
        /// 支持位域，如果是位域组合值，多个按分隔符组合。
        /// </summary>
        public static string GetDescription(this Enum @enum)
        {
            return EnumDic.GetOrAdd(@enum, (key) =>
            {
                var type = key.GetType();
                var field = type.GetField(key.ToString());
                //如果field为null则应该是组合位域值，
                return field == null ? key.ToString() : GetDescription(field);
            });
        }

        private static string GetDescription(MemberInfo member)
        {
            var att = member.GetCustomAttribute<DescriptionAttribute>(false);
            return att == null ? member.Name : att.Description;
        }
    }
}
