using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iQQ.Net.WebQQCore.Util.Extensions
{
    public static class DictionaryExtensions
    {
        /// <summary>DataList的Add方法的选项</summary>
        public enum AddChoice
        {
            /// <summary>直接添加，有重复时忽略</summary>
            IgnoreDuplication,
            /// <summary>若存在，则更新数值</summary>
            Update,
            /// <summary>直接添加，有重复时异常</summary>
            NotIgnoreDuplication,
        }


        public static void AddOrDo<TKey, TValue>(this IDictionary<TKey, TValue> dic, TKey key, TValue value, Action<TKey> action = null)
        {
            if (dic.ContainsKey(key)) action?.Invoke(key);
            else dic.Add(key, value);
        }

        public static TValue GetValueOrDefault<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue defaultValue = default(TValue))
        {
            return dict.ContainsKey(key) ? dict[key] : defaultValue;
        }


        /// <summary>更新：如果不存在，则添加；存在，则替换。
        /// <para>不更新：如果不存在，则添加；存在，则忽略。</para></summary>
        public static void Add<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue value, AddChoice addChoice)
        {
            switch (addChoice)
            {
                case AddChoice.Update:
                    dict[key] = value;
                    return;

                case AddChoice.IgnoreDuplication:
                    return;

                case AddChoice.NotIgnoreDuplication:
                    throw new ArgumentException("已存在具有相同键的元素");
            }
        }

        public static void Add<TList, TKey, TValue>(this Dictionary<TKey, TList> dic, TKey key, TValue value) where TList : IList<TValue>, new()
        {
            if (dic.ContainsKey(key))
            {
                dic[key].Add(value);
            }
            else
            {
                dic.Add(key, new TList { value });
            }
        }
    }
}
