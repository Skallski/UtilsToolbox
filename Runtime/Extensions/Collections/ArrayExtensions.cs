namespace SkalluUtils.Extensions.Collections
{
    public static class ArrayExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="array"></param>
        /// <param name="value"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static int IndexOf<T>(this T[] array, T value)
        {
            return System.Array.IndexOf(array, value);
        }  
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="array"></param>
        /// <param name="match"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static int FindIndex<T>(this T[] array, System.Predicate<T> match)
        {
            return System.Array.FindIndex(array, match);
        }

        /// <summary>
        /// Creates a subset of an array
        /// </summary>
        /// <param name="array"> array on which the method will be called </param>
        /// <param name="startIndex"> index that will be the first element of subset </param>
        /// <param name="length"> amount of elements that will be added one-by-one to the subset </param>
        /// <typeparam name="T">  type of the array </typeparam>
        /// <returns> array, which is a subset of original processed array </returns>
        public static T[] Subset<T>(this T[] array, int startIndex, int length)
        {
            T[] subset = new T[length];
            System.Array.Copy(array, startIndex, subset, 0, length);
            
            return subset;
        }

        /// <summary>
        /// Creates a subset of an array
        /// </summary>
        /// <param name="array"> array on which the method will be called </param>
        /// <param name="indices"> indices </param>
        /// <typeparam name="T">  type of the array </typeparam>
        /// <returns> array, which is a subset of original processed array </returns>
        public static T[] Subset<T>(this T[] array, params int[] indices)
        {
            int len = indices.Length;
            T[] subset = new T[len];

            for (int i = 0; i < len; i++)
            {
                subset[i] = array[indices[i]];
            }

            return subset;
        }
    }
}