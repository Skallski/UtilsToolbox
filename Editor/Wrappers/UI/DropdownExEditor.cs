using SkalluUtils.Utils.UI.Wrappers;
using UnityEditor;

namespace SkalluUtils.Wrappers.UI
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