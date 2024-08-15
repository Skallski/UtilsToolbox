using UnityEditor;
using UtilsToolbox.Utils.MultiSwitch;

namespace UtilsToolbox.Editor.CustomEditors.MultiSwitch
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(MultiSwitchWithParams<,>), true)]
    public class MultiSwitchWithParamsEditor : MultiSwitchEditor
    {
        private SerializedProperty _source;
        private SerializedProperty _values;

        protected override void OnEnable()
        {
            base.OnEnable();
            
            _source = serializedObject.FindProperty("_source");
            _values = serializedObject.FindProperty("_values");
        }

        protected override void DrawGui()
        {
            base.DrawGui();
            
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(_source);
            EditorGUILayout.PropertyField(_values);
        }

        protected override bool IsStateValid()
        {
            return base.IsStateValid() && StateToSet < _values.arraySize;
        }
    }
}