using System.Collections.Concurrent;
using System.Linq.Expressions;

namespace Utility.Helpers
{
    public static class LambdaHelper<T>
    {
        private static readonly ConcurrentDictionary<string, LambdaExpression> Cache =
            new ConcurrentDictionary<string, LambdaExpression>();

        public static LambdaExpression GetPropertyLambdaExp(string propertyName)
        {
            return Cache.GetOrAdd(propertyName, (key) =>
            {
                var param = Expression.Parameter(typeof(T));
                var body = Expression.Property(param, propertyName);
                var exp = Expression.Lambda(body, param);
                return exp;
            });
        }
    }
}
