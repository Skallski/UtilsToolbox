namespace SkalluUtils.Extensions
{
    public static class ObjectExtensions
    {
        /// <summary>
        /// Tries to cast object to provided result
        /// </summary>
        /// <example>
        /// <code>
        /// if (gameObject.TryCast(out T result))
        /// {
        ///     // You can use T result here
        /// }
        /// else
        /// {
        ///     // The cast failed.
        /// }
        /// </code>>
        /// </example>>
        /// <param name="obj"> object to cast </param>
        /// <param name="result"> expected result to get after cast </param>
        /// <typeparam name="T"> Generic type of result parameter </typeparam>
        /// <returns> true or false, depending on whether object casting worked or not </returns>
        public static bool TryCast<T>(this object obj, out T result) where T : class
        {
            result = obj as T;
            return result != null;
        }
    }
}