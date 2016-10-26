using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace Utility.Extensions
{
    public static class DescriptionExtensions
    {
        private static readonly ConcurrentDictionary<Type, string> TypeCache =
       new ConcurrentDictionary<Type, string>();

        private static readonly ConcurrentDictionary<MemberInfo, string> MemberCache =
            new ConcurrentDictionary<MemberInfo, string>();

        private static string GetDescription(MemberInfo propertyMember)
        {
            return MemberCache.GetOrAdd(propertyMember, key =>
            {
                var attr = key.GetCustomAttribute<DescriptionAttribute>();
                return attr?.Description ?? key.Name;
            });
        }

        public static string GetDescription(this Type type)
        {
            return TypeCache.GetOrAdd(type, key =>
            {
                var attr = type.GetTypeInfo().GetCustomAttribute<DescriptionAttribute>();
                return attr?.Description ?? key.Name;
            });
        }

        public static string GetDescription<TModel>(this TModel model)
        {
            return GetDescription(typeof(TModel));
        }

        public static string GetDescription<TModel, TProperty>(this TModel model, Expression<Func<TModel, TProperty>> expression)
        {
            var memberExpression = expression.Body as MemberExpression;
            var member = memberExpression?.Member;
            return GetDescription(member);
        }
    }
}
