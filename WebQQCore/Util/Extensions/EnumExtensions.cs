using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace iQQ.Net.WebQQCore.Util.Extensions
{
    public static class EnumExtension
    {
        private static readonly ConcurrentDictionary<Enum, string> _enumDic = new ConcurrentDictionary<Enum, string>();
        private static readonly ConcurrentDictionary<Type, string> _typeDic = new ConcurrentDictionary<Type, string>();

        /// <summary>
        /// 获取枚举的描述信息(Descripion)。
        /// 支持位域，如果是位域组合值，多个按分隔符组合。
        /// </summary>
        public static string GetDescription(this Enum @enum)
        {
            return _enumDic.GetOrAdd(@enum, (key) =>
            {
                var type = key.GetType();
                var field = type.GetField(key.ToString());
                //如果field为null则应该是组合位域值，
                return field == null ? key.ToString() : GetDescription(field);
            });
        }

        private static string GetDescription(this MemberInfo member)
        {
            var att = member.GetCustomAttribute<DescriptionAttribute>(false);
            return att == null ? member.Name : att.Description;
        }

        private static string GetDescription(this Type type)
        {
            var att = type.GetCustomAttribute<DescriptionAttribute>(false);
            return att == null ? type.Name : att.Description;
        }

        public static string GetFullDescription(this Enum en)
        {
            var type = en.GetType();
            var typeDesc = _typeDic.GetOrAdd(type, (key) => GetDescription(type));
            var enumDesc = en.GetDescription();
            return $"{typeDesc}-{enumDesc}";
        }
    }
}
