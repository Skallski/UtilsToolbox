using UnityEditor;

namespace Main.Scripts.Utils.MultiSwitch.Editor
{
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

        protected override void ValidateStateEdition()
        {
            base.ValidateStateEdition();

            if (StateToSet >= _values.arraySize)
            {
                StateToSet = _values.arraySize - 1;
            }
        }
    }
}