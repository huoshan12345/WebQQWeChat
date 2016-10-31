using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Utility.Extensions
{
    public static class TypeExtensions
    {
        public static Type GetUnderlyingType(this Type type)
        {
            return Nullable.GetUnderlyingType(type) ?? type;
        }

        public static bool SequenceAssignableFrom(this IEnumerable<Type> first, IEnumerable<Type> second)
        {
            var comparer = EqualityComparer<Type>.Default;
            using (var e1 = first.GetEnumerator())
            {
                using (var e2 = second.GetEnumerator())
                {
                    while (e1.MoveNext())
                    {
                        if (!e2.MoveNext()) return false;
                        else if (!(comparer.Equals(e1.Current, e2.Current) || e1.Current.IsAssignableFrom(e2.Current)))
                            return false;
                    }
                    if (e2.MoveNext())
                        return false;
                }
            }
            return true;
        }
    }
}
