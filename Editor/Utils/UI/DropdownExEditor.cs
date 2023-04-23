using UnityEditor;

namespace SkalluUtils.Utils.UI
{
    [CustomEditor(typeof(DropdownTmp))]
    [CanEditMultipleObjects]
    public class DropdownExEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
        }
    }
}