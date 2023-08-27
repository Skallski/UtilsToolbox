using SkalluUtils.Utils.UI;
using UnityEditor;

namespace SkalluUtils.CustomEditors.UI
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