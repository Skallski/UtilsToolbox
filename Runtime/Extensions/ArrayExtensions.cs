
namespace SkalluUtils.Extensions
{
    public static class ArrayExtensions
    {
        public static T[] Subset<T>(this T[] array, int startIndex, int length)
        {
            T[] subset = new T[length];
            System.Array.Copy(array, startIndex, subset, 0, length);
            
            return subset;
        }

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