using UnityEditor;

namespace SkalluUtils.Utils.UI
{
    [CustomEditor(typeof(DropdownEx))]
    [CanEditMultipleObjects]
    public class DropdownExEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
        }
    }
}