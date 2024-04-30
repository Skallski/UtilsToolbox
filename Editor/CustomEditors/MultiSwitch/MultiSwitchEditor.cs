using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Main.Scripts.Utils.MultiSwitch.Editor
{
    [CustomEditor(typeof(MultiSwitch))]
    public class MultiSwitchEditor : UnityEditor.Editor
    {
        private MultiSwitch _multiSwitch;
        
        private SerializedProperty _defaultStateOnAwake;
        private SerializedProperty _state;
        
        protected int StateToSet;
        private bool _editMode;

        protected virtual void OnEnable()
        {
            _multiSwitch = target as MultiSwitch;

            _defaultStateOnAwake = serializedObject.FindProperty("_defaultStateOnAwake");
            _state = serializedObject.FindProperty("_state");

            _editMode = false;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.BeginVertical();

            DrawGui();
            
            EditorGUILayout.EndVertical();
            serializedObject.ApplyModifiedProperties();
        }

        protected virtual void DrawGui()
        {
            EditorGUILayout.PropertyField(_defaultStateOnAwake);

            EditorGUILayout.BeginHorizontal();
            if (_editMode)
            {
                StateToSet = EditorGUILayout.IntField("State", StateToSet,
                    GUILayout.MinWidth(50), GUILayout.ExpandWidth(true));
                ValidateStateEdition();
                
                if (GUILayout.Button("Set", GUILayout.MinWidth(50), GUILayout.ExpandWidth(true)))
                {
                    MethodInfo setStateMethod = _multiSwitch.GetType().GetMethods().FirstOrDefault(method =>
                        method.Name.Equals("SetState") && method.GetParameters().Length == 1 &&
                        method.GetParameters()[0].ParameterType == typeof(int));

                    setStateMethod?.Invoke(_multiSwitch, new object[] { StateToSet });
                }
                
                if (GUILayout.Button("Cancel", GUILayout.MinWidth(50), GUILayout.ExpandWidth(true)))
                {
                    _editMode = false;
                }
            }
            else
            {
                GUI.enabled = false;
                EditorGUILayout.PropertyField(_state);
                GUI.enabled = true;
                
                if (GUILayout.Button("Edit", GUILayout.MinWidth(50), GUILayout.ExpandWidth(true)))
                {
                    _editMode = !_editMode;
                }
            }
            
            EditorGUILayout.EndHorizontal();
        }

        protected virtual void ValidateStateEdition()
        {
            if (StateToSet < 0)
            {
                StateToSet = 0;
            }
        }
    }
}