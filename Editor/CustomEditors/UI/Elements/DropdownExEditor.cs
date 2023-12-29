using SkalluUtils.Utils.UI.Elements;
using UnityEditor;

namespace SkalluUtils.CustomEditors.UI.Elements
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