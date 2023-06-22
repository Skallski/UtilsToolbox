using System.Collections.Generic;

namespace SkalluUtils.Extensions
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
                throw new System.IndexOutOfRangeException("Cannot select random item from empty list!");

            return list[UnityEngine.Random.Range(0, list.Count)];
        }

        /// <summary>
        /// Gets random index from list
        /// </summary>
        /// <param name="list"> list on which the method will be called </param>
        /// <typeparam name="T"> type of list </typeparam>
        /// <returns> random index from list </returns>
        public static int RandomIndex<T>(this IList<T> list)
        {
            if (list.Count == 0)
                throw new System.IndexOutOfRangeException("Cannot select random index from empty list!");

            return UnityEngine.Random.Range(0, list.Count);
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
                T value = list[j];
                list[j] = list[i];
                list[i] = value;
            }
        }
        
        /// <summary>
        /// Check if the list contains all elements of second list
        /// </summary>
        /// <param name="a"> list on which the method will be called </param>
        /// <param name="b"> list to check if "A" contains all of its elements </param>
        /// <typeparam name="T"> type of list </typeparam>
        /// <returns> true or false, depending on whether the list contains all elements of the second list </returns>
        public static bool ContainsAll<T>(this IList<T> a, IEnumerable<T> b)
        {
            bool contains = false;

            foreach (T element in b)
            {
                if (a.Contains(element))
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