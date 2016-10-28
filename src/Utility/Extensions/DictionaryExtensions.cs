using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Utility.Extensions
{
    public static class DictionaryExtensions
    {
        public static TValue GetOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dic, TKey key, TValue defaultValue = default(TValue))
        {
            return dic.ContainsKey(key) ? dic[key] : defaultValue;
        }

        public static void AddOrDo<TKey, TValue>(this IDictionary<TKey, TValue> dic, TKey key, TValue value, Action<TKey> action = null)
        {
            if (dic.ContainsKey(key)) action?.Invoke(key);
            else dic.Add(key, value);
        }

        public static void Add<TList, TKey, TValue>(this IDictionary<TKey, TList> dic, TKey key, TValue value) where TList : IList<TValue>, new()
        {
            if (dic.ContainsKey(key) && dic[key] != null)
            {
                dic[key].Add(value);
            }
            else
            {
                dic[key] = new TList { value };
            }
        }

        public static string ToQueryString(this IDictionary<string, string> dic)
        {
            return dic.IsNullOrEmpty() ? string.Empty :
                string.Join("&", dic.Select(item => $"{item.Key.UrlEncode()}={item.Value.UrlEncode()}"));
        }
    }
}
