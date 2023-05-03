namespace SkalluUtils.Utils.ObjectCulling
{
    public interface ICullable
    {
        void OnVisible();
        void OnInvisible();
    }
}