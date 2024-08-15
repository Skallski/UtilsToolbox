using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace UtilsToolbox.Editor.CustomEditors.MultiSwitch
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(UtilsToolbox.Utils.MultiSwitch.MultiSwitch))]
    public class MultiSwitchEditor : UnityEditor.Editor
    {
        private UtilsToolbox.Utils.MultiSwitch.MultiSwitch _multiSwitch;
        
        private SerializedProperty _defaultStateOnAwake;
        private SerializedProperty _state;
        
        protected int StateToSet;
        private bool _editMode;

        protected virtual void OnEnable()
        {
            _multiSwitch = target as UtilsToolbox.Utils.MultiSwitch.MultiSwitch;

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

            if (Application.isPlaying)
            {
                if (_editMode)
                {
                    EditorGUILayout.BeginHorizontal();
                
                    StateToSet = EditorGUILayout.IntField("State", StateToSet,
                        GUILayout.MinWidth(50), GUILayout.ExpandWidth(true));

                    if (GUILayout.Button(new GUIContent(EditorGUIUtility.IconContent("P4_CheckOutRemote").image),
                            GUILayout.MinWidth(50), GUILayout.ExpandWidth(true)))
                    {
                        if (IsStateValid())
                        {
                            CallStateChange(StateToSet);
                        }
                    }

                    if (GUILayout.Button(new GUIContent(EditorGUIUtility.IconContent("P4_DeletedLocal").image),
                            GUILayout.MinWidth(50), GUILayout.ExpandWidth(true)))
                    {
                        _editMode = false;
                    }
                
                    EditorGUILayout.EndHorizontal();
                    
                    EditorGUILayout.HelpBox("Preview mode will not affect MultiSwitch outside the play mode",
                        MessageType.Info);
                }
                else
                {
                    EditorGUILayout.BeginHorizontal();

                    GUI.enabled = false;
                    EditorGUILayout.PropertyField(_state);
                    GUI.enabled = true;

                    if (GUILayout.Button("Preview", GUILayout.MinWidth(50), GUILayout.ExpandWidth(true)))
                    {
                        _editMode = true;
                    }
                
                    EditorGUILayout.EndHorizontal();
                }
            }
            else
            {
                GUI.enabled = false;
                EditorGUILayout.PropertyField(_state);
                GUI.enabled = true;
            }
        }

        protected virtual bool IsStateValid()
        {
            return StateToSet >= 0;
        }

        private void CallStateChange(int state)
        {
            MethodInfo setStateMethod = _multiSwitch.GetType().GetMethods().FirstOrDefault(method =>
                method.Name.Equals("SetState") && method.GetParameters().Length == 1 &&
                method.GetParameters()[0].ParameterType == typeof(int));

            setStateMethod?.Invoke(_multiSwitch, new object[] { state });
        }
    }
}