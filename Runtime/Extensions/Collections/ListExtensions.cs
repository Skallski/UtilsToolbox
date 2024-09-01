using System.Collections.Generic;

namespace UtilsToolbox.Extensions.Collections
{
    public static class ListExtensions
    {
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
        /// Creates a subset of an array
        /// </summary>
        /// <param name="list"> array on which the method will be called </param>
        /// <param name="startIndex"> index that will be the first element of subset </param>
        /// <param name="endIndex"> index that will be the last element of subset </param>
        /// <typeparam name="T">  type of the array </typeparam>
        /// <returns> iEnumerable, which is a subset of original processed array </returns>
        public static IList<T> Subset<T>(this IList<T> list, int startIndex, int endIndex)
        {
            if (startIndex < 0 || endIndex >= list.Count || startIndex > endIndex)
            {
                throw new System.ArgumentOutOfRangeException("Invalid start index or end index");
            }

            int count = endIndex - startIndex + 1;
            T[] result = new T[count];
            for (int i = 0; i < count; i++)
            {
                result[i] = list[startIndex + i];
            }

            return result;
        }
    }
}