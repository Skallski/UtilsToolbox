namespace SkalluUtils.Utils.DataBridge.Wrappers
{
    [System.Serializable]
    public class DataArrayWrapper<T>
    {
        public T[] Array;

        public DataArrayWrapper(T[] array)
        {
            Array = array;
        }
    }
}