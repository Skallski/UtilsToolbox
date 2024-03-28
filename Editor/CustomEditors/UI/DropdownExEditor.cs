using SkalluUtils.Utils.UI;
using UnityEditor;

namespace SkalluUtils.Editor.CustomEditors.UI
{
    [CustomEditor(typeof(DropdownTmp))]
    [CanEditMultipleObjects]
    public class DropdownExEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
        }
    }
}