using UnityEngine;

namespace SkalluUtils.Extensions
{
    namespace Vector3Extensions
    {
        public static class Vector3Extensions
        {
            /// <summary>
            /// Sets X parameter of the 3-dimensional vector
            /// </summary>
            /// <param name="vector"> vector on which the method will be called </param>
            /// <param name="x"> x value to set </param>
            public static void SetX(this Vector3 vector, float x) => vector = new Vector3(x, vector.y, vector.z);

            /// <summary>
            /// Sets Y parameter of the 3-dimensional vector
            /// </summary>
            /// <param name="vector"> vector on which the method will be called </param>
            /// <param name="y"> y value to set </param>
            public static void SetY(this Vector3 vector, float y) => vector = new Vector3(vector.x, y, vector.z);

            /// <summary>
            /// Sets Z parameter of the 3-dimensional vector
            /// </summary>
            /// <param name="vector"> vector on which the method will be called </param>
            /// <param name="z"> z value to set </param>
            public static void SetZ(this Vector3 vector, float z) => vector = new Vector3(vector.x, vector.y, z);

            /// <summary>
            /// Gets the square distance between two 3-dimensional vector positions
            /// </summary>
            /// <param name="first"> vector on which the method will be called </param>
            /// <param name="second"> second position </param>
            /// <returns> float value that is squared distance between two points </returns>
            public static float SqrDistance(this Vector3 first, Vector3 second) =>
                (first.x - second.x) * (first.x - second.x) +
                (first.y - second.y) * (first.y - second.y) +
                (first.z - second.z) * (first.z - second.z);
        
        }
    }
    
    namespace Vector2Extensions
    {
        public static class Vector2Extensions
        {
            /// <summary>
            /// Sets X parameter of the 2-dimensional vector
            /// </summary>
            /// <param name="vector"> vector on which the method will be called </param>
            /// <param name="x"> x value to set </param>
            public static void SetX(this Vector2 vector, float x) => vector = new Vector2(x, vector.y);
        
            /// <summary>
            /// Sets Y parameter of the 2-dimensional vector
            /// </summary>
            /// <param name="vector"> vector on which the method will be called </param>
            /// <param name="y"> y value to set </param>
            public static void SetY(this Vector2 vector, float y) => vector = new Vector2(vector.x, y);
        
            /// <summary>
            /// Gets the square distance between two 2-dimensional vector positions
            /// </summary>
            /// <param name="first"> vector on which the method will be called </param>
            /// <param name="second"> second position </param>
            /// <returns> float value that is squared distance between two points </returns>
            public static float SqrDistance(this Vector2 first, Vector2 second) =>
                (first.x - second.x) * (first.x - second.x) +
                (first.y - second.y) * (first.y - second.y);
        
        }
    }
}