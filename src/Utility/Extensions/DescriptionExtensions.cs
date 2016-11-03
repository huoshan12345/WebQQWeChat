using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Utility.Extensions
{
    public static class DescriptionExtensions
    {
        private static readonly ConcurrentDictionary<Enum, string> EnumDic = new ConcurrentDictionary<Enum, string>();
        private static readonly ConcurrentDictionary<Type, string> TypeDic = new ConcurrentDictionary<Type, string>();
        private static readonly ConcurrentDictionary<MemberInfo, string> MemberDic = new ConcurrentDictionary<MemberInfo, string>();

        /// <summary>
        /// 获取枚举的描述信息(Descripion)
        /// </summary>
        public static string GetDescription(this Enum @enum)
        {
            return EnumDic.GetOrAdd(@enum, (key) =>
            {
                var type = key.GetType();
                var field = type.GetTypeInfo().GetField(key.ToString());
                //如果field为null则应该是组合位域值，
                return field == null ? key.ToString() : GetDescription(field);
            });
        }

        public static string GetDescription(this MemberInfo member)
        {
            return MemberDic.GetOrAdd(member, (key) =>
            {
                var att = member.GetCustomAttribute<DescriptionAttribute>(false);
                return att == null ? member.Name : att.Description;
            });
        }

        public static string GetDescription(this Type type)
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
    }
}
