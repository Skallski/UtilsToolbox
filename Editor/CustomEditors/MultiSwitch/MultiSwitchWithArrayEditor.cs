using UnityEditor;

namespace Main.Scripts.Utils.MultiSwitch.Editor
{
    [CustomEditor(typeof(MultiSwitchWithArray<>), true)]
    public class MultiSwitchWithArrayEditor : MultiSwitchEditor
    {
        private SerializedProperty _elements;
        
        protected override void OnEnable()
        {
            base.OnEnable();
            
            _elements = serializedObject.FindProperty("_elements");
        }

        protected override void DrawGui()
        {
            base.DrawGui();
            
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(_elements);
        }
    }
}