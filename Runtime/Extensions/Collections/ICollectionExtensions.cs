using System.Collections.Generic;

namespace UtilsToolbox.Extensions.Collections
{
    public static class ICollectionExtensions
    {
        /// <summary>
        /// Check if the list is null or empty
        /// </summary>
        /// <param name="list"> list on which the method will be called </param>
        /// <typeparam name="T"> type of list </typeparam>
        /// <returns> true or false, depending on whether the list is null or empty </returns>
        public static bool IsNullOrEmpty<T>(this ICollection<T> collection)
        {
            if (collection == null)
            {
                return true;
            }

            if (collection.Count == 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Check if list is a subset of other list
        /// </summary>
        /// <param name="subset"> list on which the method will be called </param>
        /// <param name="set"> list to check if it contains whole subset </param>
        /// <typeparam name="T"> type of both lists </typeparam>
        /// <returns> true or false, depending on whether the list is a subset of second list </returns>
        public static bool IsSubsetOf<T>(this ICollection<T> subset, ICollection<T> set)
        {
            bool contains = false;
            
            foreach (T element in subset)
            {
                if (set.Contains(element))
                {
                    contains = true;
                }
                else
                {
                    return false;
                }
            }

            return contains;
        }
    }
}