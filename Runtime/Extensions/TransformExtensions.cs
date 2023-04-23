using System.Collections.Generic;
using UnityEngine;

namespace SkalluUtils.Extensions
{
    public static class TransformExtensions
    {
        /// <summary>
        /// Gets all children of transform
        /// </summary>
        /// <param name="transform"> transform on which the method will be called </param>
        /// <returns> list of children of transform </returns>
        public static List<Transform> Children(this Transform transform)
        {
            List<Transform> children = new List<Transform>();
            
            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                children.Add(transform.GetChild(i));
            }

            return children;
        }
    }
}