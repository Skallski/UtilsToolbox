using UnityEditor;
using UtilsToolbox.Utils.UI;

namespace UtilsToolbox.Editor.CustomEditors.UI
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