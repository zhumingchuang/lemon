using System.Collections.Generic;

namespace LemonFramework
{
    /// <summary>
    /// 集合扩展
    /// </summary>
    public static class CollectionsExtension
    {
        /// <summary>
        /// 尝试设置值
        /// </summary>
        public static bool TrySetValue<TKey, TValue> (this IDictionary<TKey, TValue> dic, TKey key, TValue value)
        {
            var ison = dic.ContainsKey (key);
            if (ison)
            {
                dic[key] = value;
            }
            return ison;
        }

        /// <summary>
        /// 获取值
        /// 若不存在返回默认
        /// </summary>
        public static TValue GetValueAnyway<TKey, TValue> (this IDictionary<TKey, TValue> dic, TKey key)
        {
            TValue tValue = default (TValue);
            dic.TryGetValue (key, out tValue);
            return tValue;
        }

        /// <summary>
        /// 尝试获取不存在的值 如果不存在添加默认值
        /// </summary>
        public static Tvalue TryGetMissingValue<Tkey, Tvalue> (this IDictionary<Tkey, Tvalue> dict, Tkey key, Tvalue value)
        {
            if (dict.ContainsKey (key))
            {
                if (dict[key] == null)
                {
                    dict[key] = value;
                }
            }
            else
            {
                dict.Add (key, value);
            }
            return dict[key];
        }

        /// <summary>
        /// 获取数组值
        /// 若不存在返回默认
        /// </summary>
        public static T GetValueAnyway<T> (this IList<T> arr, int index)
        {
            T t = default (T);
            arr.TryGetValue<T> (index, out t);
            return t;
        }

        /// <summary>
        /// 是否合法的索引
        /// </summary>
        public static bool IsValid<T> (this IList<T> arr, int index)
        {
            if (arr == null || index < 0)
            {
                return false;
            }
            return index < arr.Count;
        }

        /// <summary>
        /// 尝试获取数组中的值
        /// </summary>
        public static bool TryGetValue<T> (this IList<T> arr, int index, out T t)
        {
            bool flag = false;
            if (arr == null || index < 0 || index >= arr.Count)
            {
                t = default (T);
            }
            else
            {
                t = arr[index];
                flag = true;
            }
            return flag;
        }

        /// <summary>
        /// 尝试设置数组中的值
        /// </summary>
        public static bool TrySetValue<T> (this IList<T> arr, int index, T t)
        {
            bool flag = false;
            if (arr != null && index >= 0 && index < arr.Count)
            {
                arr[index] = t;
                flag = true;
            }
            return flag;
        }
    }
}