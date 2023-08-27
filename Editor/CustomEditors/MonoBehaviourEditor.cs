using SkalluUtils.Utils;
using UnityEditor;
using UnityEngine;

namespace SkalluUtils.CustomEditors
{
    [CustomEditor(typeof(MonoBehaviour), true)]
    public class MonoBehaviourEditor : Editor
    {
        private bool _methodsShown;
        private MethodButtonSerialization.CustomMethodInfo[] _methodsToShow;

        public override void OnInspectorGUI()
        {
            MethodButtonSerialization.DrawMethods(target, ref _methodsToShow, ref _methodsShown);
            EditorGUIExtensions.DoDrawDefaultInspector(serializedObject);
        }
    }
}