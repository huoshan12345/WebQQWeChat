using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace Utility.Extensions
{
    public static class MemberInfoExtensions
    {
        public static MemberInfo GetMemberInfo<TModel, TProperty>(this TModel model, Expression<Func<TModel, TProperty>> expression)
        {
            var memberExpression = expression.Body as MemberExpression;
            var member = memberExpression?.Member;
            return member;
        }

        public static MemberInfo GetMemberInfo<TModel, TProperty>(Expression<Func<TModel, TProperty>> expression)
        {
            return GetMemberInfo(default(TModel), expression);
        }
    }
}
