using System.Collections.Generic;

namespace SkalluUtils.Extensions.Collections
{
    public static class DictionaryExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="value"></param>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <returns></returns>
        public static List<T> GetAllKeysByValue<T, TValue>(this IDictionary<T, TValue> dict, TValue value)
        {
            List<T> keys = new List<T>();
            
            foreach (KeyValuePair<T, TValue> kvp in dict)
            {
                if (EqualityComparer<TValue>.Default.Equals(kvp.Value, value)) 
                {
                    keys.Add(kvp.Key);
                }
            }
            
            return keys;
        }
    }
}