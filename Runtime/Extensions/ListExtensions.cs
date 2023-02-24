using System.Collections.Generic;

namespace SkalluUtils.Extensions
{
    namespace ListExtensions
    {
        public static class ListExtensions
        {
            /// <summary>
            /// Gets random item from list
            /// </summary>
            /// <param name="list"> list on which the method will be called </param>
            /// <typeparam name="T"> generic type of list </typeparam>
            /// <returns> random list item </returns>
            public static T RandomItem<T>(this IList<T> list)
            {
                int len = list.Count;
                if (len == 0)
                    throw new System.IndexOutOfRangeException("Cannot select random item from empty list!");

                return list[UnityEngine.Random.Range(0, len)];
            }

            /// <summary>
            /// Shuffles list
            /// </summary>
            /// <param name="list"> list on which the method will be called </param>
            /// <typeparam name="T"> generic type of list </typeparam>
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
            /// Check if list contains all elements of second list
            /// </summary>
            /// <param name="a"> list on which the method will be called </param>
            /// <param name="b"> list to check if "A" contains all of its elements </param>
            /// <typeparam name="T"> generic type of list </typeparam>
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
        }
    }
}