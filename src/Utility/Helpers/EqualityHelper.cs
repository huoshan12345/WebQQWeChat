using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Utility.Helpers
{
    public static class EqualityHelper<T>
    {
        public static IEqualityComparer<T> CreateComparer<V>(Func<T, V> keySelector, IEqualityComparer<V> comparer = null)
        {
            return new CommonEqualityComparer<V>(keySelector, comparer);
        }

        class CommonEqualityComparer<V> : IEqualityComparer<T>
        {
            private readonly Func<T, V> _keySelector;
            private readonly IEqualityComparer<V> _comparer;

            public CommonEqualityComparer(Func<T, V> keySelector, IEqualityComparer<V> comparer)
            {
                _keySelector = keySelector;
                _comparer = comparer ?? EqualityComparer<V>.Default;
            }

            public bool Equals(T x, T y)
            {
                return _comparer.Equals(_keySelector(x), _keySelector(y));
            }
            public int GetHashCode(T obj)
            {
                return _comparer.GetHashCode(_keySelector(obj));
            }
        }
    }
}
