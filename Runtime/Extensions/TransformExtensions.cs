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

        /// <summary>
        /// Rotates transform towards destination position over time with certain speed
        /// </summary>
        /// <param name="transform"> transform on which the method will be called </param>
        /// <param name="rotationDestination"> destination, to which transform component will be rotated </param>
        /// <param name="rotationSpeed"> rotation speed multiplier </param>
        public static void RotateToOverTime(this Transform transform, Vector3 rotationDestination, float rotationSpeed)
        {
            Vector3 direction = (rotationDestination - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x);
            Quaternion desiredRotation = Quaternion.AngleAxis(angle * Mathf.Rad2Deg, Vector3.forward);
            
            transform.rotation = Quaternion.RotateTowards(transform.rotation, desiredRotation,
                Time.deltaTime * rotationSpeed);
        }
    }
}