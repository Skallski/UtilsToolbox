using System.Collections.Generic;
using JetBrains.Annotations;

namespace SkalluUtils.Extensions.Collections
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
        /// Gets random index from list
        /// </summary>
        /// <param name="list"> list on which the method will be called </param>
        /// <typeparam name="T"> type of list </typeparam>
        /// <returns> random index from list </returns>
        public static int RandomIndex<T>(this IList<T> list)
        {
            if (list.Count == 0)
            {
                throw new System.IndexOutOfRangeException("Cannot select random index from empty list!");
            }

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
                (list[j], list[i]) = (list[i], list[j]);
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

        /// <summary>
        /// Gets element
        /// </summary>
        /// <param name="list"></param>
        /// <param name="desiredElement"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetElement<[CanBeNull] T>(this IList<T> list, T desiredElement)
        {
            EqualityComparer<T> equalityComparer = EqualityComparer<T>.Default;
            
            for (int i = 0, c = list.Count; i < c; i++)
            {
                T currentElement = list[i];
                if (equalityComparer.Equals(currentElement, desiredElement))
                {
                    return currentElement;
                }
            }
            
            return default;
        }

        public static List<T> GetElements<[CanBeNull] T>(this IList<T> list, T desiredElement)
        {
            List<T> elements = new List<T>();
            EqualityComparer<T> equalityComparer = EqualityComparer<T>.Default;
            
            for (int i = 0, c = list.Count; i < c; i++)
            {
                T currentElement = list[i];
                if (equalityComparer.Equals(currentElement, desiredElement))
                {
                    elements.Add(currentElement);
                }
            }
            
            return elements;
        }
    }
}