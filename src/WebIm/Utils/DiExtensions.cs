using System;
using System.Collections.Generic;
using System.Text;
using FclEx;
using Microsoft.Extensions.DependencyInjection;

namespace WebIm.Utils
{
    public static class DiExtensions
    {
        public static IServiceCollection AddSingletonIf<T>(this IServiceCollection col,
            T obj, bool condition) where T : class
        {
            if (condition)
                col.AddSingleton(obj);
            return col;
        }

        public static IServiceCollection AddSingletonIfNotNull<T>(this IServiceCollection col, T obj)
            where T : class
        {
            return AddSingletonIf(col, obj, obj.IsNotNull());
        }

        public static IServiceCollection AddSingletonIfNotNull<T>(this IServiceCollection col,
            Func<IServiceProvider, T> fac)
            where T : class
        {
            return AddSingletonIf(col, fac, fac.IsNotNull());
        }

        public static IServiceCollection AddSingletonIf<T>(this IServiceCollection col,
            Func<IServiceProvider, T> fac, bool condition)
            where T : class
        {
            if (condition)
                col.AddSingleton(fac);
            return col;
        }
    }
}
