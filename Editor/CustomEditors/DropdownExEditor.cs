using SkalluUtils.Utils.UI;
using UnityEditor;

namespace SkalluUtils.CustomEditors
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