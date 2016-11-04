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
                var str = key.ToString();
                var field = key.GetType().GetTypeInfo().GetField(str);
                return field == null ? str : GetDescription(field);
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
            var typeDesc = TypeDic.GetOrAdd(type, GetDescription);
            var enumDesc = en.GetDescription();
            return $"{typeDesc}-{enumDesc}";
        }
    }
}
