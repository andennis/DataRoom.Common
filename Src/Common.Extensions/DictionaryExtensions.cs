using System.Collections.Generic;

namespace Common.Extensions
{
    public static class DictionaryExtensions
    {
        public static void AddRange<TKey, TValue>(this IDictionary<TKey, TValue> dst, IDictionary<TKey, TValue> src, bool isSrcImportant = true)
        {
            if (isSrcImportant)
            {
                foreach (KeyValuePair<TKey, TValue> srcItem in src)
                    dst[srcItem.Key] = srcItem.Value;
            }
            else
            {
                foreach (KeyValuePair<TKey, TValue> srcItem in src)
                {
                    if (!dst.ContainsKey(srcItem.Key))
                        dst[srcItem.Key] = srcItem.Value;
                }
            }
        }
    }
}
