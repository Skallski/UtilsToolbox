using UnityEngine;

namespace SkalluUtils.Extensions
{
    public static class ColliderExtensions
    {
        /// <summary>
        /// Gets random point inside collider bounds
        /// </summary>
        /// <param name="col"> collider on which the method will be called </param>
        public static Vector3 RandomPointInsideBounds(this Collider col)
        {
            Bounds bounds = col.bounds;
                
            return new Vector3(
                Random.Range(bounds.min.x, bounds.max.x),
                Random.Range(bounds.min.y, bounds.max.y),
                Random.Range(bounds.min.z, bounds.max.z));
        }
    }
    
    public static class Collider2DExtensions
    {
        /// <summary>
        /// Gets random point inside collider bounds
        /// </summary>
        /// <param name="col"> collider2D on which the method will be called </param>
        public static Vector2 RandomPointInsideBounds(this Collider2D col)
        {
            Bounds bounds = col.bounds;
                
            return new Vector2(
                Random.Range(bounds.min.x, bounds.max.x),
                Random.Range(bounds.min.y, bounds.max.y));
        }
    }
}