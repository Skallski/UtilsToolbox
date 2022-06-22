using System.Collections.Generic;
using UnityEngine;

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
            public static T RandomItem<T>(this IList<T> list) => list[Random.Range(0, list.Count)];
            
            /// <summary>
            /// Shuffles list
            /// </summary>
            /// <param name="list"> list on which the method will be called </param>
            /// <typeparam name="T"> generic type of list </typeparam>
            public static void Shuffle<T>(this IList<T> list)
            {
                for (var i = list.Count - 1; i > 1; i--)
                {
                    var j = Random.Range(0, i + 1);
                    var value = list[j];
                    list[j] = list[i];
                    list[i] = value;
                }
            }
        }
    }
}