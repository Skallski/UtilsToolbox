using System.Collections.Generic;

namespace UtilsToolbox.Extensions.Collections
{
    public static class ListExtensions
    {
        /// <summary>
        /// Gets random item from list
        /// </summary>
        /// <param name="list"> list on which the method will be called </param>
        /// <typeparam name="T"> type of list </typeparam>
        /// <returns> random item from list </returns>
        public static T RandomItem<T>(this IList<T> list)
        {
            if (list.Count == 0)
            {
                throw new System.IndexOutOfRangeException("Cannot select random item from empty list!");
            }

            return list[UnityEngine.Random.Range(0, list.Count)];
        }

        /// <summary>
        /// Shuffles list
        /// </summary>
        /// <param name="list"> list on which the method will be called </param>
        /// <typeparam name="T"> type of list </typeparam>
        public static void Shuffle<T>(this IList<T> list)
        {
            int len = list.Count;
            for (int i = len - 1; i > 1; i--)
            {
                int j = UnityEngine.Random.Range(0, i + 1);
                (list[j], list[i]) = (list[i], list[j]);
            }
        }

        /// <summary>
        /// Check if list is a subset of other list
        /// </summary>
        /// <param name="subset"> list on which the method will be called </param>
        /// <param name="set"> list to check if it contains whole subset </param>
        /// <typeparam name="T"> type of both lists </typeparam>
        /// <returns> true or false, depending on whether the list is a subset of second list </returns>
        public static bool IsSubsetOf<T>(this IList<T> subset, IList<T> set)
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

        /// <summary>
        /// Removes list of elements from selected list
        /// </summary>
        /// <param name="origin"> list from which elements will be removed </param>
        /// <param name="partToRemove"> list that contains elements that will be removed </param>
        /// <typeparam name="T"> type of list </typeparam>
        public static void RemoveRange<T>(this IList<T> origin, IEnumerable<T> partToRemove)
        {
            if (origin.Count <= 0)
            {
                return;
            }

            foreach (T t in partToRemove)
            {
                if (origin.Contains(t))
                {
                    origin.Remove(t);
                }
            }
        }

        /// <summary>
        /// Check if the list is null or empty
        /// </summary>
        /// <param name="list"> list on which the method will be called </param>
        /// <typeparam name="T"> type of list </typeparam>
        /// <returns> true or false, depending on whether the list is null or empty </returns>
        public static bool IsNullOrEmpty<T>(this IList<T> list)
        {
            if (list == null)
            {
                return true;
            }

            if (list.Count == 0)
            {
                return true;
            }

            return false;
        }

    }
}