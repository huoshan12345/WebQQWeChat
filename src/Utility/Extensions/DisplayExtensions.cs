using System;
using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace Utility.Extensions
{
    public static class DisplayExtensions
    {
        private static readonly ConcurrentDictionary<MemberInfo, string> MemberCache =
            new ConcurrentDictionary<MemberInfo, string>();

        public static string GetDisplayName(this MemberInfo propertyMember)
        {
            return MemberCache.GetOrAdd(propertyMember, key =>
            {
                var attr = key.GetCustomAttribute<DisplayAttribute>();
                return attr?.Name ?? key.Name;
            });
        }

        public static string GetDisplayName<TModel, TProperty>(this TModel model, Expression<Func<TModel, TProperty>> expression)
        {
            var memberExpression = expression.Body as MemberExpression;
            var member = memberExpression?.Member;
            return GetDisplayName(member);
        }

        public static string GetDisplayName<TModel, TProperty>(Expression<Func<TModel, TProperty>> expression)
        {
            return GetDisplayName(default(TModel), expression);
        }

        public static string GetDisplayFormatString<TModel, TProperty>(this TModel model, Expression<Func<TModel, TProperty>> expression)
        {
            var memberExpression = (MemberExpression)expression.Body;
            var member = (PropertyInfo)memberExpression.Member;
            var attr = member.GetCustomAttribute<DisplayFormatAttribute>();
            var value = member.GetValue(model);
            return attr.DataFormatString == null ? value.ToString() : string.Format(attr.DataFormatString, value);

        }
    }
}
