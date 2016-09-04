using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace iQQ.Net.WebQQCore.Util.Extensions
{
    public static class EnumExtension
    {
        private static readonly ConcurrentDictionary<Enum, string> EnumDic = new ConcurrentDictionary<Enum, string>();
        private static readonly ConcurrentDictionary<Type, string> TypeDic = new ConcurrentDictionary<Type, string>();
        private static readonly ConcurrentDictionary<MemberInfo, string> MemberDic = new ConcurrentDictionary<MemberInfo, string>();

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

        private static string GetDescription(this MemberInfo member)
        {
            return MemberDic.GetOrAdd(member, (key) =>
            {
                var att = member.GetCustomAttribute<DescriptionAttribute>(false);
                return att == null ? member.Name : att.Description;
            });
        }

        private static string GetDescription(this Type type)
        {
            return TypeDic.GetOrAdd(type, (key) =>
            {
                var att = type.GetTypeInfo().GetCustomAttribute<DescriptionAttribute>(false);
                return att == null ? type.Name : att.Description;
            });
        }

        public static string GetFullDescription(this Enum en)
        {
            var type = en.GetType();
            var typeDesc = TypeDic.GetOrAdd(type, (key) => GetDescription(type));
            var enumDesc = en.GetDescription();
            return $"{typeDesc}-{enumDesc}";
        }

        public static IEnumerable<T> GetValues<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<T>();
        }


    }
}
