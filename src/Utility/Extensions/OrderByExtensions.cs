using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Utility.Helpers;

namespace Utility.Extensions
{
    public static class OrderByExtensions
    {
        private static IOrderedQueryable<T> OrderingHelper<T>(IQueryable<T> source, string propertyName, bool descending, bool anotherLevel)
        {
            var param = Expression.Parameter(typeof(T));
            var property = Expression.Property(param, propertyName);
            var sort = LambdaHelper<T>.GetPropertyLambdaExp(propertyName);
            var call = Expression.Call(typeof(Queryable),
                (!anotherLevel ? "OrderBy" : "ThenBy") + (descending ? "Descending" : string.Empty),
                new[] { typeof(T), property.Type },
                source.Expression,
                Expression.Quote(sort));
            var query = (IOrderedQueryable<T>)source.Provider.CreateQuery<T>(call);
            return query;
        }

        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, string propertyName)
        {
            return OrderingHelper(source, propertyName, false, false);
        }

        public static IOrderedQueryable<T> OrderByIf<T>(this IQueryable<T> source, string propertyName, bool condition)
        {
            return condition ? OrderingHelper(source, propertyName, false, false) : (IOrderedQueryable<T>)source;
        }

        private static IOrderedQueryable<T> OrderByDescending<T>(this IQueryable<T> source, string propertyName)
        {
            return OrderingHelper(source, propertyName, true, false);
        }

        private static IOrderedQueryable<T> ThenBy<T>(this IOrderedQueryable<T> source, string propertyName)
        {
            return OrderingHelper(source, propertyName, false, true);
        }

        private static IOrderedQueryable<T> ThenByDescending<T>(this IOrderedQueryable<T> source, string propertyName)
        {
            return OrderingHelper(source, propertyName, true, true);
        }
    }
}
