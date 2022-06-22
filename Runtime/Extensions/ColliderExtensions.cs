using UnityEngine;

namespace SkalluUtils.Extensions
{
    namespace ColliderExtensions
    {
        public static class ColliderExtensions
        {
            /// <summary>
            /// Gets random point inside collider bounds
            /// </summary>
            /// <param name="col"> collider on which the method will be called </param>
            public static Vector3 RandomPointInsideBounds(this Collider col)
            {
                return new Vector3(
                    Random.Range(col.bounds.min.x, col.bounds.max.x),
                    Random.Range(col.bounds.min.y, col.bounds.max.y),
                    Random.Range(col.bounds.min.z, col.bounds.max.z));
            }
        }
    }
    
    namespace Collider2DExtensions
    {
        public static class Collider2DExtensions
        {
            /// <summary>
            /// Gets random point inside collider bounds
            /// </summary>
            /// <param name="col"> collider2D on which the method will be called </param>
            public static Vector2 RandomPointInsideBounds(this Collider2D col)
            {
                return new Vector2(
                    Random.Range(col.bounds.min.x, col.bounds.max.x),
                    Random.Range(col.bounds.min.y, col.bounds.max.y));
            }
        }
    }
}